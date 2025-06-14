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

        public GroupController(IGroupService groupService, IAccountService accountService)
        {
            _groupService = groupService;
            _accountService = accountService;
        }

        [Authorize]
        [HttpPost("createGroup")]
        public StatusCodeResult CreateGroup(CreateGroupRequestDto createGroupRequestDto)
        {
            var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!int.TryParse(claimValue, out var currentAccountId))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(currentAccountId);
            if (account == null || account.DefaultUser == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var createdGroup = _groupService.CreateGroup(createGroupRequestDto.Name, account.DefaultUser.Id);

            if (createdGroup != null)
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
