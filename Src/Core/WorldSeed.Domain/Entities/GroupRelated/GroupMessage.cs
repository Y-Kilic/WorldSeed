using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.CharacterRelated;

namespace WorldSeed.Domain.Entities.GroupRelated
{
    public class GroupMessage
    {
        public int Id { get; set; }
        public Group Sender { get; set; }
        public Group Receiver { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
