using WorldSeed.Domain.Entities.GroupRelated;

namespace WorldSeed.Application.DTOS
{
    public class GroupMemberDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public GroupRank Rank { get; set; }
    }
}
