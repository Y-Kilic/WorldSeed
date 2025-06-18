using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Domain.Entities.UserRelated;

#nullable enable

namespace WorldSeed.Persistence.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfwork;

        public AccountService(IUnitOfWork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }

        public Account CreateAccount(string username, string email, byte[] passwordHash, byte[] passwordSalt)
        {
            var userNameExist = _unitOfwork.Accounts.Find(u => u.UserName.Equals(username)).FirstOrDefault();

            if(userNameExist != null)
            {
                return null;
            }

            var emailExist = _unitOfwork.Accounts.Find(u => u.Email.Equals(email)).FirstOrDefault();

            if (emailExist != null)
            {
                return null;
            }

            var newAccount = new Account()
            {
                UserName = username,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _unitOfwork.Accounts.Add(newAccount);

            _unitOfwork.SaveChanges();

            return newAccount;
        }

        public Account CheckLoginByEmail(string email, string password)
        {
            var accountFromDB = _unitOfwork.Accounts.GetAll().Where(u => u.Email.Equals(email)).FirstOrDefault();

            // Account not exist
            if(null == accountFromDB)
            {
                return null;
            }

            // Check user password if valid
            var result = VerifyPasswordHash(password, accountFromDB.PasswordHash, accountFromDB.PasswordSalt);

            // If valid return account.
            if(result == true)
            {
                return accountFromDB;
            }

            // not valid login, return null.
            return null;
        }

        public Account GetAccountByEmail(string email)
        {
            return _unitOfwork.Accounts.GetAll().FirstOrDefault(a => a.Email.Equals(email));
        }
        public Account GetAccountByUsername(string username)
        {
            return _unitOfwork.Accounts.GetAll().FirstOrDefault(a => a.UserName.Equals(username));
        }
        public Account GetAccountById(int accountId)
        {
            var account = _unitOfwork.Accounts.Get(accountId);
            if (account != null && account.DefaultUserId.HasValue)
            {
                account.DefaultUser = _unitOfwork.Users.Get(account.DefaultUserId.Value);
            }
            return account;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public void UpdateTokens(int accountId, string refreshToken, DateTime expires, DateTime created)
        {
            var account = _unitOfwork.Accounts.GetAll().Where(u => u.Id.Equals(accountId)).FirstOrDefault();

            account.RefreshToken = refreshToken;
            account.TokenExpires = expires;
            account.TokenCreated = created;

            _unitOfwork.SaveChanges();
        }

        public User? GetDefaultUser(int accountId)
        {
            var account = GetAccountById(accountId);
            return account?.DefaultUser;
        }

        public bool SetDefaultUser(int accountId, int userId)
        {
            var account = _unitOfwork.Accounts.Get(accountId);
            var user = _unitOfwork.Users.GetAll()
                .FirstOrDefault(u => u.Id == userId && u.Account.Id == accountId);
            if (account == null || user == null)
            {
                return false;
            }

            account.DefaultUser = user;
            account.DefaultUserId = user.Id;
            _unitOfwork.SaveChanges();
            return true;
        }
    }
}
