using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool CreateForum(string name, int? groupId)
        {
            if (!groupId.HasValue)
            {
                return false;
            }

            var group = _unitOfWork.Groups.Get(groupId.Value);
            if (group == null)
            {
                return false;
            }

            var forum = new Forum
            {
                Name = name,
                Group = group,
                GroupId = group.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            group.Forum = forum;

            _unitOfWork.Forums.Add(forum);
            _unitOfWork.SaveChanges();
            return true;
        }

        public ForumCategory CreateCategory(int forumId, string name)
        {
            var forum = _unitOfWork.Forums.Get(forumId);
            if (forum == null)
            {
                return null!;
            }

            var category = new ForumCategory
            {
                Forum = forum,
                ForumId = forum.Id,
                Name = name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _unitOfWork.ForumCategories.Add(category);
            _unitOfWork.SaveChanges();
            return category;
        }

        public ForumCategoryThread CreateThread(int forumCategoryId, int ownerId, string title)
        {
            var category = _unitOfWork.ForumCategories.Get(forumCategoryId);
            var owner = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Id == ownerId);
            if (category == null || owner == null)
            {
                return null!;
            }

            var thread = new ForumCategoryThread
            {
                ForumCategory = category,
                ForumCategoryId = category.Id,
                Owner = owner,
                OwnerId = owner.Id,
                Title = title,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _unitOfWork.ForumCategoryThreads.Add(thread);
            _unitOfWork.SaveChanges();
            return thread;
        }

        public ForumCategoryThreadPost CreatePost(int forumCategoryThreadId, int ownerId, string content)
        {
            var thread = _unitOfWork.ForumCategoryThreads.Get(forumCategoryThreadId);
            var owner = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Id == ownerId);
            if (thread == null || owner == null)
            {
                return null!;
            }

            var post = new ForumCategoryThreadPost
            {
                ForumCategoryThread = thread,
                ForumCategoryThreadId = thread.Id,
                Owner = owner,
                OwnerId = owner.Id,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _unitOfWork.ForumCategoryThreadPosts.Add(post);
            _unitOfWork.SaveChanges();
            return post;
        }

        public Forum GetForum(int id)
        {
            return _unitOfWork.Forums.Get(id);
        }

        public IEnumerable<ForumCategory> GetCategories(int forumId)
        {
            return _unitOfWork.ForumCategories.Find(c => c.ForumId == forumId);
        }

        public IEnumerable<ForumCategoryThread> GetThreads(int categoryId)
        {
            return _unitOfWork.ForumCategoryThreads.Find(t => t.ForumCategoryId == categoryId);
        }

        public IEnumerable<ForumCategoryThreadPost> GetPosts(int threadId)
        {
            return _unitOfWork.ForumCategoryThreadPosts.Find(p => p.ForumCategoryThreadId == threadId);
        }
    }
}
