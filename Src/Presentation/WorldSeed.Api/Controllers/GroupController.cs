using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        private readonly IAccountService _accountService;

        [Authorize]
        [HttpPost("createGroup")]
        public StatusCodeResult CreateGroup(CreateGroupRequestDto createGroupRequestDto)
        {
            var currentAccountId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var defaultAccountUser = _accountService.GetAccountById(currentAccountId).DefaultUser;

            var createdGroup = _groupService.CreateGroup(createGroupRequestDto.Name, defaultAccountUser.Id);

            if (createdGroup != null)
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
