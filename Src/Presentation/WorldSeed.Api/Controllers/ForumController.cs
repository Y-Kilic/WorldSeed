using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
        private readonly IAccountService _accountService;

        public ForumController(IForumService forumService, IAccountService accountService)
        {
            _forumService = forumService;
            _accountService = accountService;
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

        [Authorize]
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

        [Authorize]
        [HttpPost("createThread")]
        public ActionResult<ForumCategoryThread> CreateThread(CreateForumThreadDTO dto)
        {
            if (dto.OwnerId <= 0)
            {
                var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
                if (int.TryParse(claimValue, out var accountId))
                {
                    var account = _accountService.GetAccountById(accountId);
                    if (account != null && account.DefaultUser != null)
                    {
                        dto.OwnerId = account.DefaultUser.Id;
                    }
                }
            }

            var result = _forumService.CreateThread(dto);
            if (result == null)
            {
                return BadRequest();
            }
            return Created($"/api/forum/thread/{result.Id}", result);
        }

        [Authorize]
        [HttpPost("createPost")]
        public ActionResult<ForumCategoryThreadPost> CreatePost(CreateForumPostDTO dto)
        {
            if (dto.OwnerId <= 0)
            {
                var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
                if (int.TryParse(claimValue, out var accountId))
                {
                    var account = _accountService.GetAccountById(accountId);
                    if (account != null && account.DefaultUser != null)
                    {
                        dto.OwnerId = account.DefaultUser.Id;
                    }
                }
            }

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
