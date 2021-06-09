using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> Register(AppUser appUser);
        Task<bool> IsExists(string username);
        Task<bool> SaveAllAsync();
    }
}