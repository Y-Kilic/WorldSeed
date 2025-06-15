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
        public int ForumCategoryId { get; set; }
        public ForumCategory ForumCategory { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
