using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace API.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<bool> Register(AppUser appUser);
        Task<bool> IsExists(string username);
        Task<bool> SaveAllAsync();
    }
}