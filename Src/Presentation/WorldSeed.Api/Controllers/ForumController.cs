using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces.Services;

namespace WorldSeed.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly IForumService _forumService;

        [HttpPost("createForum")]
        public StatusCodeResult CreateGroup(CreateForumDTO createForumDTO)
        {
            _forumService.CreateForum(createForumDTO);

            if (_forumService.CreateForum(createForumDTO))
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
