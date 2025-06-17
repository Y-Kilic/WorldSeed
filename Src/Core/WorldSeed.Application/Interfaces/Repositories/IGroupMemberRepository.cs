using System.Collections.Generic;
using WorldSeed.Domain.Entities.GroupRelated;

namespace WorldSeed.Application.Interfaces.Repositories
{
    public interface IGroupMemberRepository : IRepository<GroupMember>
    {
        IEnumerable<GroupMember> GetMembersWithGroups(long userId);
    }
}
