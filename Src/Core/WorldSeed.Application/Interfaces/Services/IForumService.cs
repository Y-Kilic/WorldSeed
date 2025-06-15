using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.DTOS;
using WorldSeed.Domain.Entities.ForumRelated;

namespace WorldSeed.Application.Interfaces.Services
{
    public interface IForumService
    {
        bool CreateForum(CreateForumDTO createForumDTO);
        ForumCategory CreateCategory(CreateForumCategoryDTO dto);
        ForumCategoryThread CreateThread(CreateForumThreadDTO dto);
        ForumCategoryThreadPost CreatePost(CreateForumPostDTO dto);
        Forum GetForum(int id);
        IEnumerable<ForumCategory> GetCategories(int forumId);
        IEnumerable<ForumCategoryThread> GetThreads(int categoryId);
        IEnumerable<ForumCategoryThreadPost> GetPosts(int threadId);
    }
}
