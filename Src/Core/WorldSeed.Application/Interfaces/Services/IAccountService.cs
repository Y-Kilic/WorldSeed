using System;
using WorldSeed.Application.DTOS;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Persistence.Services;

namespace WorldSeed.Application.Interfaces.Services
{
    public interface IAccountService
    {
        public bool CreateAccount(string username, string email, byte[] passwordHash, byte[] passwordSalt);
        public Account CheckLoginByEmail(string email, string password);
        public Account GetAccountById(int accountId);
        public Account GetAccountByEmail(string email);
        public Account GetAccountByUsername(string username);
        public void UpdateTokens(int accountId, string refreshToken, DateTime Expires, DateTime Created);
    }
}