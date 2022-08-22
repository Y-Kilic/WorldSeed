using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSeed.Domain.Entities.AccountRelated
{
    public class AccountMessage
    {
        public int Id { get; set; }
        public Account Sender { get; set; }
        public Account Receiver { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
