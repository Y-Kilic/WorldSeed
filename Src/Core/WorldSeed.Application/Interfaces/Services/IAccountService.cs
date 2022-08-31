using System;
using WorldSeed.Application.DTOS;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Persistence.Services;

namespace WorldSeed.Application.Interfaces.Services
{
    public interface IAccountService
    {
        public bool CreateAccount(CreateAccountDTO createAccountDTO);
        public Account CheckLoginByEmail(string email, string password);
        public Account GetAccountByEmail(string email);
        public Account GetAccountByUsername(string username);
        public void UpdateTokens(string accountEmail, string refreshToken, DateTime Expires, DateTime Created);
    }
}