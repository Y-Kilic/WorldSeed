using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.DTOS;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Application.Interfaces.Services
{
    public interface IUserService
    {
        public User CreateUser(int accountId, string username);
    }
}
