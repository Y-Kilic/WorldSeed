using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.GroupRelated;
using WorldSeed.Domain.Interfaces.Repositories;
using WorldSeed.Infrastructure.Data;

namespace WorldSeed.Infrastructure.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context) : base(context)
        {
        }

        ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
