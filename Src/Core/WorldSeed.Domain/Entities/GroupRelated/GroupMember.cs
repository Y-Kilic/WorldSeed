using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.UserRelated;
using WorldSeed.Domain.Entities.AccountRelated;

namespace WorldSeed.Domain.Entities.GroupRelated
{
    public class GroupMember
    {
        public int Id { get; set; }
        public Group Group { get; set; }
        public User Character { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
