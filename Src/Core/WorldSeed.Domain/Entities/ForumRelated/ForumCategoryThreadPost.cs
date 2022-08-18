using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.CharacterRelated;

namespace WorldSeed.Domain.Entities.ForumRelated
{
    public class ForumCategoryThreadPost
    {
        public int Id { get; set; }
        public ForumCategoryThread ForumCategoryThread { get; set; }
        public Character Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
