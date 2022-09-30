using Microsoft.AspNetCore.Mvc;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Domain.Entities.GroupRelated;

namespace WorldSeed.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        [HttpPost("createGroup")]
        public StatusCodeResult CreateGroup(CreateGroupDTO createGroupDTO)
        {
            _groupService.CreateGroup(createGroupDTO);

            if (_groupService.CreateGroup(createGroupDTO))
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
