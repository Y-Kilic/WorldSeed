using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.Interfaces.Repositories;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Domain.Entities.GroupRelated;

namespace WorldSeed.Domain.Interfaces.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
    }
}
