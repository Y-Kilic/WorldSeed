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

namespace WorldSeed.Persistence.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfwork;

        public AccountService(IUnitOfWork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }

        public bool CreateAccount(CreateAccountDTO createAccountDTO)
        {
            var userNameExist = _unitOfwork.Accounts.GetAll().Where(u => u.UserName.Equals(createAccountDTO.UserName)).FirstOrDefault();

            if(userNameExist != null)
            {
                return false;
            }

            var emailExist = _unitOfwork.Accounts.GetAll().Where(u => u.Email.Equals(createAccountDTO.Email)).FirstOrDefault();

            if (emailExist != null)
            {
                return false;
            }

            var newAccount = new Account()
            {
                UserName = createAccountDTO.UserName,
                Email = createAccountDTO.Email,
                PasswordHash = createAccountDTO.PasswordHash,
                PasswordSalt = createAccountDTO.PasswordSalt
            };

            _unitOfwork.Accounts.Add(newAccount);

            _unitOfwork.SaveChanges();

            return true;
        }

        public Account CheckLogin(LoginAccountDTO loginAccountDTO)
        {
            var accountFromDB = _unitOfwork.Accounts.GetAll().Where(u => u.UserName.Equals(loginAccountDTO.UserName)).FirstOrDefault();

            // Account not exist
            if(null == accountFromDB)
            {
                return null;
            }

            // Check user password if valid
            var result = VerifyPasswordHash(loginAccountDTO.Password, accountFromDB.PasswordHash, accountFromDB.PasswordSalt);

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

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
