using WorldSeed.Domain.Entities.GroupRelated;
using WorldSeed.Domain.Interfaces.Repositories;
using WorldSeed.Infrastructure.Data;

namespace WorldSeed.Infrastructure.Repositories
{
    public class GroupMemberRepository : Repository<GroupMember>, IGroupMemberRepository
    {
        public GroupMemberRepository(ApplicationDbContext context) : base(context)
        {
        }

        ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
