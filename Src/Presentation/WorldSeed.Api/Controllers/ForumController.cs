using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Domain.Entities.ForumRelated;

namespace WorldSeed.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly IForumService _forumService;

        public ForumController(IForumService forumService)
        {
            _forumService = forumService;
        }

        [HttpPost("createForum")]
        public StatusCodeResult CreateGroup(CreateForumDTO createForumDTO)
        {
            if (!_forumService.CreateForum(createForumDTO))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("createCategory")]
        public ActionResult<ForumCategory> CreateCategory(CreateForumCategoryDTO dto)
        {
            var result = _forumService.CreateCategory(dto);
            if (result == null)
            {
                return BadRequest();
            }

            return Created($"/api/forum/{dto.ForumId}/categories", result);
        }

        [HttpPost("createThread")]
        public ActionResult<ForumCategoryThread> CreateThread(CreateForumThreadDTO dto)
        {
            var result = _forumService.CreateThread(dto);
            if (result == null)
            {
                return BadRequest();
            }
            return Created($"/api/forum/thread/{result.Id}", result);
        }

        [HttpPost("createPost")]
        public ActionResult<ForumCategoryThreadPost> CreatePost(CreateForumPostDTO dto)
        {
            var result = _forumService.CreatePost(dto);
            if (result == null)
            {
                return BadRequest();
            }
            return Created($"/api/forum/thread/{dto.ForumCategoryThreadId}/posts", result);
        }

        [HttpGet("{forumId}/categories")]
        public IEnumerable<ForumCategory> GetCategories(int forumId)
        {
            return _forumService.GetCategories(forumId);
        }

        [HttpGet("category/{categoryId}/threads")]
        public IEnumerable<ForumCategoryThread> GetThreads(int categoryId)
        {
            return _forumService.GetThreads(categoryId);
        }

        [HttpGet("thread/{threadId}/posts")]
        public IEnumerable<ForumCategoryThreadPost> GetPosts(int threadId)
        {
            return _forumService.GetPosts(threadId);
        }
    }
}
