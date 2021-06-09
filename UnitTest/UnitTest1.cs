using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Repositories;
using API.Services;
using System;
using System.Text;
using Xunit;

namespace UnitTest
{
    public class UnitTest1
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IHashService _hashService;

        public UnitTest1()
        {
            // _accountRepository = new AccountRepository();
            _hashService = new HashService();
        }

        [Fact]
        public void HashTest()
        {
            string data = "Pa$$W0rd";
            var user = new AppUser();

            byte[] array = Encoding.ASCII.GetBytes("sEOFyZclFuHDWDpSlBxV0E9XXNZ091MbpYLYLuQEBK/DPGMjcCrHOScXpP53CPG4fd7oEYhgSylGf1LFyXx+dg==");

            Assert.Equal(array[], _hashService.Hash(user, data));
        }

        [Fact]
        public void CheckHashTest()
        {
            var user = new AppUser();
            string data = "PassW0rd";

            Assert.NotEqual("sEOFyZclFuHDWDpSlBxV0E9XXNZ091MbpYLYLuQEBK/DPGMjcCrHOScXpP53CPG4fd7oEYhgSylGf1LFyXx+dg==", _hashService.CheckHash(user, data));
        }

        [Fact]
        public async void isUserExistingTest()
        {
            var failedUser = "sadas";
            Assert.True(await _accountRepository.IsExists(failedUser));

            var user = "mark";
            Assert.True(await _accountRepository.IsExists(user));
        }
    }
}
