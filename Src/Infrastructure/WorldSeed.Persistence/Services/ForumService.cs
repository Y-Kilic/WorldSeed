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
            var exists = _unitOfWork.Forums
                .Find(f => f.Name.ToLower() == createForumDTO.Name.ToLower())
                .Any();

            if (exists)
            {
                return false;
            }

            var forum = new Forum
            {
                Name = createForumDTO.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _unitOfWork.Forums.Add(forum);
            _unitOfWork.SaveChanges();
            return true;
        }
    }
}
