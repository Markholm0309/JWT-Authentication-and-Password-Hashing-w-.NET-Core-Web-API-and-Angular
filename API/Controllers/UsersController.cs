using System;
using System.IO;
using System.Threading.Tasks;
using API.Interfaces;
using DAL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILoggerService _logger;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public UsersController(SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager, 
            ILoggerService loggerService, 
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = loggerService;
            _config = configuration;
           _tokenService = tokenService;
        }

        /// <summary>
        /// Register Endpoint
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var location = GetControllerActionNames();

            try
            {
                var username = userDto.EmailAddress;
                var password = userDto.Password;

                var user = new IdentityUser { Email = username, UserName = username };

                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"{location}: {error.Code} {error.Description}");
                    }

                    return InternalError($"{location}: {username} User Registration Attempt Faild");
                }

                await _userManager.AddToRoleAsync(user, Roles.User);

                return Ok(new { result.Succeeded });
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// User Login Endpoint
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var location = GetControllerActionNames();

            try
            {
                var username = userDto.EmailAddress;
                var password = userDto.Password;

                _logger.LogInfo($"{location}: Login attempt from user {username}");
                var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

                if (result.Succeeded)
                {
                    _logger.LogInfo($"{location}: {username} Succesfully Authenticated");
                    
                    var user = await _userManager.FindByNameAsync(username);
                    var tokenString = await _tokenService.CreateToken(user);

                    return Ok(new { token = tokenString });
                }

                _logger.LogInfo($"{location}: {username} Not Authenticated");
                return Unauthorized(userDto);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }         
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong");
        }
    }
}