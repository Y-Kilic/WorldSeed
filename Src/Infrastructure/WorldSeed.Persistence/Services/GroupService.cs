using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.Interfaces;
using WorldSeed.Domain.Entities.GroupRelated;

namespace WorldSeed.Persistence.Services
{
    public class GroupService
    {
        private readonly IUnitOfWork _unitOfwork;

        public GroupService(IUnitOfWork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }

        public Group CreateGroup(string groupName, long userId)
        {
            var userFromDB = _unitOfwork.Users.GetAll().Where(u => u.Id.Equals(userId)).FirstOrDefault();

            var newGroup = new Group()
            {
                Name = groupName,
                Owner = userFromDB
            };

            _unitOfwork.Groups.Add(newGroup);
            _unitOfwork.SaveChanges();

            return newGroup;
        }
    }
}
