using WorldSeed.Domain.Entities.GroupRelated;

namespace WorldSeed.Application.DTOS
{
    public class ChangeMemberRankDto
    {
        public int GroupId { get; set; }
        public int TargetUserId { get; set; }
        public GroupRank NewRank { get; set; }
    }
}
