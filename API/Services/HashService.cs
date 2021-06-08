using System.Security.Cryptography;
using System.Text;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class HashService : IHashService
    {
        public string Hash(AppUser user, string password)
        {
            using var hmac = new HMACSHA512();

            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            user.PasswordSalt = hmac.Key;

            return user.PasswordHash.ToString();
        }
    }
}