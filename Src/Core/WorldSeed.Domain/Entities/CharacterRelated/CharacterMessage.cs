using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Domain.Entities.CharacterRelated
{
    public class CharacterMessage
    {
        public int Id { get; set; }
        public Character Sender { get; set; }
        public Character Receiver { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
