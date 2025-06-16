using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Domain.Entities.ForumRelated;

namespace WorldSeed.Application.Interfaces.Services
{
    public interface IForumService
    {
        bool CreateForum(string name, int? groupId);
        ForumCategory CreateCategory(int forumId, string name);
        ForumCategoryThread CreateThread(int forumCategoryId, int ownerId, string title);
        ForumCategoryThreadPost CreatePost(int forumCategoryThreadId, int ownerId, string content);
        Forum GetForum(int id);
        IEnumerable<ForumCategory> GetCategories(int forumId);
        IEnumerable<ForumCategoryThread> GetThreads(int categoryId);
        IEnumerable<ForumCategoryThreadPost> GetPosts(int threadId);
    }
}
