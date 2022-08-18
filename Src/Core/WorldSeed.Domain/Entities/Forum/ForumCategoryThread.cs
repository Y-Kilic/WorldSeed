using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSeed.Domain.Entities.Forum.Forum
{
    public class ForumCategoryThread
    {
        public int Id { get; set; }
        public ForumCategory ForumCategory { get; set; }
    }
}
