using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSeed.Domain.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public Forum Forum { get; set; }

    }
}
