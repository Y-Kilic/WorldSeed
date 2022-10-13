using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.Interfaces;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Persistence.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfwork;

        public UserService(IUnitOfWork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public User CreateUserAndSetAccountDefault(int accountId, string username)
        {
            var account = _unitOfwork.Accounts.Get(accountId);

            var newUser = new User()
            {
                Account = account,
                CreatedAt = DateTime.UtcNow,
                Name = username
            };

            _unitOfwork.Users.Add(newUser);
            _unitOfwork.SaveChanges();

            return newUser;
        }
    }
}
