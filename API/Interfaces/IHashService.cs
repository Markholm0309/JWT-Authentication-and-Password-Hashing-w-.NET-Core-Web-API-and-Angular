using API.Entities;

namespace API.Interfaces
{
    public interface IHashService
    {
        string Hash(AppUser user, string password);
    }
}