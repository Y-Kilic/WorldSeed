using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.DTOS;
using WorldSeed.Domain.Entities.GroupRelated;

namespace WorldSeed.Application.Interfaces.Services
{
    public interface IGroupService
    {
        public Group CreateGroup(string name, long userId);
    }
}
