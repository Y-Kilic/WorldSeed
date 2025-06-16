using System;
using WorldSeed.Application.DTOS;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Domain.Entities.UserRelated;

#nullable enable

namespace WorldSeed.Application.Interfaces.Services
{
    public interface IAccountService
    {
        public Account CreateAccount(string username, string email, byte[] passwordHash, byte[] passwordSalt);
        public Account CheckLoginByEmail(string email, string password);
        public Account GetAccountById(int accountId);
        public Account GetAccountByEmail(string email);
        public Account GetAccountByUsername(string username);
        public void UpdateTokens(int accountId, string refreshToken, DateTime Expires, DateTime Created);
        public User? GetDefaultUser(int accountId);
        public bool SetDefaultUser(int accountId, int userId);
    }
}