using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WorldSeed.Domain.Entities.GroupRelated;
using WorldSeed.Application.Interfaces.Repositories;
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

        public IEnumerable<GroupMember> GetMembersWithGroups(long userId)
        {
            return ApplicationDbContext.GroupMembers
                .Include(m => m.Group)
                .Where(m => m.User.Id == userId)
                .ToList();
        }

        public IEnumerable<GroupMember> GetMembersWithUsers(int groupId)
        {
            return ApplicationDbContext.GroupMembers
                .Include(m => m.User)
                .Where(m => m.Group.Id == groupId)
                .ToList();
        }
    }
}
