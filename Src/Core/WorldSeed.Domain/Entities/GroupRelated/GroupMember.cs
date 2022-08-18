using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Domain.Entities.GroupRelated
{
    public class GroupMember
    {
        public int Id { get; set; }
        public Group Group { get; set; }
        public User User { get; set; }

    }
}
