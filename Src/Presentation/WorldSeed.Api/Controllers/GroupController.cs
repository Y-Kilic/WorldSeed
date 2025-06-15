using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
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

        [Authorize]
        [HttpGet("mine")]
        public IEnumerable<GroupDto> GetMyGroups()
        {
            var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!int.TryParse(claimValue, out var accountId))
            {
                return Enumerable.Empty<GroupDto>();
            }

            var account = _accountService.GetAccountById(accountId);
            if (account == null || account.DefaultUser == null)
            {
                return Enumerable.Empty<GroupDto>();
            }

            return _groupService.GetGroupsForUser(account.DefaultUser.Id)
                .Select(g => new GroupDto { Id = g.Id, Name = g.Name });
        }

        [Authorize]
        [HttpGet("joinable")]
        public IEnumerable<GroupDto> GetJoinableGroups()
        {
            var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!int.TryParse(claimValue, out var accountId))
            {
                return Enumerable.Empty<GroupDto>();
            }

            var account = _accountService.GetAccountById(accountId);
            if (account == null || account.DefaultUser == null)
            {
                return Enumerable.Empty<GroupDto>();
            }

            return _groupService.GetJoinableGroups(account.DefaultUser.Id)
                .Select(g => new GroupDto { Id = g.Id, Name = g.Name });
        }

        [Authorize]
        [HttpGet("{groupId}/members")]
        public IEnumerable<GroupMemberDto> GetGroupMembers(int groupId)
        {
            var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!int.TryParse(claimValue, out var accountId))
            {
                return Enumerable.Empty<GroupMemberDto>();
            }

            var account = _accountService.GetAccountById(accountId);
            if (account == null || account.DefaultUser == null)
            {
                return Enumerable.Empty<GroupMemberDto>();
            }

            return _groupService.GetGroupMembers(groupId)
                .Select(m => new GroupMemberDto
                {
                    UserId = m.User.Id,
                    UserName = m.User.Name,
                    Rank = m.Rank
                });
        }

        [Authorize]
        [HttpPost("join")]
        public StatusCodeResult JoinGroup(JoinGroupRequestDto joinDto)
        {
            var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!int.TryParse(claimValue, out var accountId))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(accountId);
            if (account == null || account.DefaultUser == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var result = _groupService.JoinGroup(joinDto.GroupId, account.DefaultUser.Id);
            if (result != null)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [Authorize]
        [HttpPost("leave")]
        public StatusCodeResult LeaveGroup(JoinGroupRequestDto dto)
        {
            var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!int.TryParse(claimValue, out var accountId))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(accountId);
            if (account == null || account.DefaultUser == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var success = _groupService.LeaveGroup(dto.GroupId, account.DefaultUser.Id);
            return success ? StatusCode(StatusCodes.Status200OK) : StatusCode(StatusCodes.Status400BadRequest);
        }

        [Authorize]
        [HttpPost("updateName")]
        public StatusCodeResult UpdateGroupName(UpdateGroupNameDto dto)
        {
            var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!int.TryParse(claimValue, out var accountId))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(accountId);
            if (account == null || account.DefaultUser == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var success = _groupService.UpdateGroupName(dto.GroupId, dto.NewName, account.DefaultUser.Id);
            return success ? StatusCode(StatusCodes.Status200OK) : StatusCode(StatusCodes.Status400BadRequest);
        }

        [Authorize]
        [HttpPost("rank")]
        public StatusCodeResult ChangeRank(ChangeMemberRankDto dto)
        {
            var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!int.TryParse(claimValue, out var accountId))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(accountId);
            if (account == null || account.DefaultUser == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var success = _groupService.ChangeMemberRank(dto.GroupId, account.DefaultUser.Id, dto.TargetUserId, dto.NewRank);
            return success ? StatusCode(StatusCodes.Status200OK) : StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
