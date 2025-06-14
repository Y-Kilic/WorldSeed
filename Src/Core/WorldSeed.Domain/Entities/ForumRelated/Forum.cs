using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.GroupRelated;

namespace WorldSeed.Domain.Entities.ForumRelated
{
    public class Forum
    {
        public string Name { get; set; }
        public int? GroupId { get; set; }
        public Group? Group { get; set; }
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
