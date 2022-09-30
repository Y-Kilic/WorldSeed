using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Domain.Entities.ForumRelated
{
    public class ForumCategoryThread
    {
        public int Id { get; set; }
        public ForumCategory ForumCategory { get; set; }
        public User Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
