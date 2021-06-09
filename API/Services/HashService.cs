using System;
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

        public string CheckHash(AppUser user, string password)
        {   
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return HttpException(401, "Invalid Password");
            }
            
            return user.PasswordHash.ToString();
        }

        private string HttpException(int v1, string v2)
        {
            return v1 + v2;
        }
    }
}