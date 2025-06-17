using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        private int? GetAccountIdFromClaims()
        {
            var claim = Request.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "accountId");
            if (claim == null || !int.TryParse(claim.Value, out var accountId))
            {
                return null;
            }

            return accountId;
        }

        [Authorize]
        [HttpPost("createGroup")]
        public StatusCodeResult CreateGroup(CreateGroupRequestDto createGroupRequestDto)
        {
            var accountId = GetAccountIdFromClaims();
            if (!accountId.HasValue)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(accountId.Value);
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
            var accountId = GetAccountIdFromClaims();
            if (!accountId.HasValue)
            {
                return Enumerable.Empty<GroupDto>();
            }

            var account = _accountService.GetAccountById(accountId.Value);
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
            var accountId = GetAccountIdFromClaims();
            if (!accountId.HasValue)
            {
                return Enumerable.Empty<GroupDto>();
            }

            var account = _accountService.GetAccountById(accountId.Value);
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
            var accountId = GetAccountIdFromClaims();
            if (!accountId.HasValue)
            {
                return Enumerable.Empty<GroupMemberDto>();
            }

            var account = _accountService.GetAccountById(accountId.Value);
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
            var accountId = GetAccountIdFromClaims();
            if (!accountId.HasValue)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(accountId.Value);
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
            var accountId = GetAccountIdFromClaims();
            if (!accountId.HasValue)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(accountId.Value);
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
            var accountId = GetAccountIdFromClaims();
            if (!accountId.HasValue)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(accountId.Value);
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
            var accountId = GetAccountIdFromClaims();
            if (!accountId.HasValue)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(accountId.Value);
            if (account == null || account.DefaultUser == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var success = _groupService.ChangeMemberRank(dto.GroupId, account.DefaultUser.Id, dto.TargetUserId, dto.NewRank);
            return success ? StatusCode(StatusCodes.Status200OK) : StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
