using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.AccountRelated;

namespace WorldSeed.Domain.Entities.UserRelated
{
    public class User
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public String Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
