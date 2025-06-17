using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorldSeed.Api.Controllers;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Domain.Entities.UserRelated;
using WorldSeed.Domain.Entities.GroupRelated;

namespace WorldSeed.Tests;

public class GroupControllerTests
{
    private class StubGroupService : IGroupService
    {
        public Group CreateGroup(string name, long userId) => new Group { Id = 1, Name = name };
        public IEnumerable<Group> GetGroupsForUser(long userId) => throw new NotImplementedException();
        public IEnumerable<Group> GetJoinableGroups(long userId) => throw new NotImplementedException();
        public IEnumerable<GroupMember> GetGroupMembers(int groupId) => throw new NotImplementedException();
        public GroupMember JoinGroup(int groupId, long userId) => throw new NotImplementedException();
        public bool LeaveGroup(int groupId, long userId) => throw new NotImplementedException();
        public bool UpdateGroupName(int groupId, string newName, long actorUserId) => throw new NotImplementedException();
        public bool ChangeMemberRank(int groupId, long actorUserId, long targetUserId, GroupRank newRank) => throw new NotImplementedException();
    }

    private class StubAccountService : IAccountService
    {
        public Account GetAccountById(int accountId) => new Account { Id = accountId, DefaultUser = new User { Id = 2, Name = "user" } };
        public Account CreateAccount(string username, string email, byte[] passwordHash, byte[] passwordSalt) => throw new NotImplementedException();
        public Account CheckLoginByEmail(string email, string password) => throw new NotImplementedException();
        public Account GetAccountByEmail(string email) => throw new NotImplementedException();
        public Account GetAccountByUsername(string username) => throw new NotImplementedException();
        public void UpdateTokens(int accountId, string refreshToken, DateTime expires, DateTime created) => throw new NotImplementedException();
        public User? GetDefaultUser(int accountId) => new User { Id = 2, Name = "user" };
        public bool SetDefaultUser(int accountId, int userId) => throw new NotImplementedException();
    }

    [Fact]
    public void CreateGroup_WithAccountIdClaim_ReturnsCreated()
    {
        var controller = new GroupController(new StubGroupService(), new StubAccountService());
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("accountId", "1") }))
            }
        };

        var result = controller.CreateGroup(new CreateGroupRequestDto { Name = "MyGroup" });

        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public void CreateGroup_WithoutAccountIdClaim_ReturnsBadRequest()
    {
        var controller = new GroupController(new StubGroupService(), new StubAccountService());
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) } };

        var result = controller.CreateGroup(new CreateGroupRequestDto { Name = "MyGroup" });

        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }
}
