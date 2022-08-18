using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSeed.Domain.Entities.UserRelated
{
    public class UserMessage
    {
        public int Id { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
