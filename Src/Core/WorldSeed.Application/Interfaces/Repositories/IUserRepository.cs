using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
