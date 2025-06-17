using Microsoft.EntityFrameworkCore;
using WorldSeed.Infrastructure.Data;
using WorldSeed.Persistence.Services;
using WorldSeed.Domain.Entities.GroupRelated;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Tests;

public class GroupServiceTests
{
    private static GroupService CreateService(ApplicationDbContext context)
    {
        var uow = new UnitOfWork(context);
        return new GroupService(uow);
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    private static ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public void CreateGroup_ShouldPersistGroup()
    {
        using var context = CreateContext();
        var user = new User { Id = 1, Name = "owner", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Users.Add(user);
        context.SaveChanges();
        var service = CreateService(context);

        var group = service.CreateGroup("My Group", user.Id);

        Assert.NotNull(group);
        Assert.Equal("My Group", group.Name);
        Assert.Equal(1, context.Groups.Count());
        var membership = context.GroupMembers.FirstOrDefault();
        Assert.NotNull(membership);
        Assert.Equal(GroupRank.Owner, membership!.Rank);
        Assert.Equal(user.Id, membership.User.Id);
    }

    [Fact]
    public void JoinGroup_ShouldAddMembership()
    {
        using var context = CreateContext();
        var owner = new User { Id = 1, Name = "owner", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var member = new User { Id = 2, Name = "member", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Users.AddRange(owner, member);
        context.SaveChanges();
        var service = CreateService(context);

        var group = service.CreateGroup("Group", owner.Id);
        var membership = service.JoinGroup(group.Id, member.Id);

        Assert.NotNull(membership);
        Assert.Equal(GroupRank.Member, membership.Rank);
        Assert.Equal(2, context.GroupMembers.Count());
    }

    [Fact]
    public void LeaveGroup_ShouldRemoveMembership()
    {
        using var context = CreateContext();
        var owner = new User { Id = 1, Name = "owner", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var member = new User { Id = 2, Name = "member", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Users.AddRange(owner, member);
        context.SaveChanges();
        var service = CreateService(context);

        var group = service.CreateGroup("Group", owner.Id);
        service.JoinGroup(group.Id, member.Id);

        var result = service.LeaveGroup(group.Id, member.Id);

        Assert.True(result);
        Assert.Single(context.GroupMembers);
    }

    [Fact]
    public void UpdateGroupName_ByOwner_ShouldUpdate()
    {
        using var context = CreateContext();
        var owner = new User { Id = 1, Name = "owner", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Users.Add(owner);
        context.SaveChanges();
        var service = CreateService(context);

        var group = service.CreateGroup("Old", owner.Id);

        var result = service.UpdateGroupName(group.Id, "New", owner.Id);

        Assert.True(result);
        Assert.Equal("New", context.Groups.First().Name);
    }

    [Fact]
    public void ChangeMemberRank_ByOwner_ShouldUpdateRank()
    {
        using var context = CreateContext();
        var owner = new User { Id = 1, Name = "owner", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var member = new User { Id = 2, Name = "member", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Users.AddRange(owner, member);
        context.SaveChanges();
        var service = CreateService(context);

        var group = service.CreateGroup("Group", owner.Id);
        service.JoinGroup(group.Id, member.Id);

        var result = service.ChangeMemberRank(group.Id, owner.Id, member.Id, GroupRank.Admin);

        Assert.True(result);
        var updated = context.GroupMembers.First(m => m.User.Id == member.Id);
        Assert.Equal(GroupRank.Admin, updated.Rank);
    }

    [Fact]
    public void GetGroupMembers_AfterOwnerLeavesAndNewUserJoins_ReturnsUsers()
    {
        var dbName = Guid.NewGuid().ToString();
        using (var context = CreateContext(dbName))
        {
            var owner = new User { Id = 1, Name = "owner", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var newUser = new User { Id = 2, Name = "member", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            context.Users.AddRange(owner, newUser);
            context.SaveChanges();

            var service = CreateService(context);
            var group = service.CreateGroup("Group", owner.Id);
            service.LeaveGroup(group.Id, owner.Id);
            service.JoinGroup(group.Id, newUser.Id);
        }

        using (var context = CreateContext(dbName))
        {
            var service = CreateService(context);
            var members = service.GetGroupMembers(context.Groups.First().Id).ToList();

            Assert.Single(members);
            Assert.NotNull(members[0].User);
            Assert.Equal("member", members[0].User.Name);
        }
    }

    [Fact]
    public void GetGroupMembers_AfterOwnerLeavesAndRejoins_ReturnsUsers()
    {
        var dbName = Guid.NewGuid().ToString();
        using (var context = CreateContext(dbName))
        {
            var owner = new User { Id = 1, Name = "owner", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            context.Users.Add(owner);
            context.SaveChanges();

            var service = CreateService(context);
            var group = service.CreateGroup("Group", owner.Id);
            service.LeaveGroup(group.Id, owner.Id);
            service.JoinGroup(group.Id, owner.Id);
        }

        using (var context = CreateContext(dbName))
        {
            var service = CreateService(context);
            var members = service.GetGroupMembers(context.Groups.First().Id).ToList();

            Assert.Single(members);
            Assert.NotNull(members[0].User);
            Assert.Equal("owner", members[0].User.Name);
        }
    }

    [Fact]
    public void GetGroupMembers_AfterNewUserLeaves_ReturnsOwnerOnly()
    {
        var dbName = Guid.NewGuid().ToString();
        using (var context = CreateContext(dbName))
        {
            var owner = new User { Id = 1, Name = "owner", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var newUser = new User { Id = 2, Name = "member", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            context.Users.AddRange(owner, newUser);
            context.SaveChanges();

            var service = CreateService(context);
            var group = service.CreateGroup("Group", owner.Id);
            service.JoinGroup(group.Id, newUser.Id);
            service.LeaveGroup(group.Id, newUser.Id);
        }

        using (var context = CreateContext(dbName))
        {
            var service = CreateService(context);
            var members = service.GetGroupMembers(context.Groups.First().Id).ToList();

            Assert.Single(members);
            Assert.Equal("owner", members[0].User.Name);
        }
    }
}
