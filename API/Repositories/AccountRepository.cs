using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        public AccountRepository(DataContext dataContext, IMapper mapper)
        {
            _mapper = mapper;
            _db = dataContext;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync() => await _db.Users
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
                
        public async Task<bool> Register(AppUser user)
        {
            await _db.Users.AddAsync(user);

            return await SaveAllAsync();
        }

        public async Task<bool> IsExists(string username) => await _db.Users.AnyAsync(x => x.UserName == username.ToLower());

        public async Task<bool> SaveAllAsync() => await _db.SaveChangesAsync() > 0;
    }
}