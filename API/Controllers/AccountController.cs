using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly DataContext _db;
        private readonly IAccountRepository _accountRepository;
        private readonly IHashService _hashService;

        public AccountController(IAccountRepository accountRepository, DataContext dataContext,
            ITokenService tokenService, IHashService hashService)
        {
            _hashService = hashService;
            _accountRepository = accountRepository;
            _db = dataContext;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _accountRepository.GetAllUsersAsync();

            return Ok(users.Adapt<IEnumerable<AppUser>, IEnumerable<UserDto>>());
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = registerDto.Adapt<RegisterDto, AppUser>();

            user.UserName = registerDto.Username.ToLower();

            _hashService.Hash(user, registerDto.Password);

            await _accountRepository.Register(user);

            var returnDto = user.Adapt<AppUser, UserDto>();
            returnDto.Token = _tokenService.CreateToken(user);

            return StatusCode(StatusCodes.Status201Created, returnDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _accountRepository.IsExists(username);
        }
    }
}