using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSeed.Domain.Entities.ForumRelated
{
    public class ForumCategory
    {
        public int Id { get; set; }
        public Forum Forum { get; set; }
    }
}
