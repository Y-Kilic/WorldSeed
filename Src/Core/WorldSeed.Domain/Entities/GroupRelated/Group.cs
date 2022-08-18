using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.ForumRelated;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Domain.Entities.GroupRelated
{
    public class Group
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public Forum Forum { get; set; }

    }
}
