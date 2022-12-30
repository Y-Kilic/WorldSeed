using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.AccountRelated;

namespace WorldSeed.Application.DTOS
{
    public class GetAccountUsersResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
