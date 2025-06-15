using System;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Domain.Entities.ForumRelated;

namespace WorldSeed.Persistence.Services
{
    public class ForumService : IForumService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ForumService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateForum(CreateForumDTO createForumDTO)
        {
            var forum = new Forum
            {
                Name = createForumDTO.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (createForumDTO.GroupId.HasValue)
            {
                var group = _unitOfWork.Groups.Get(createForumDTO.GroupId.Value);
                if (group == null)
                {
                    return false;
                }
                forum.Group = group;
                forum.GroupId = group.Id;
                group.Forum = forum;
            }

            _unitOfWork.Forums.Add(forum);
            _unitOfWork.SaveChanges();
            return true;
        }
    }
}
