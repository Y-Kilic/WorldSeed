using Microsoft.EntityFrameworkCore;
using WorldSeed.Infrastructure.Data;
using WorldSeed.Persistence.Services;
using WorldSeed.Application.DTOS;
using WorldSeed.Domain.Entities.GroupRelated;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Tests;

public class ForumServiceTests
{
    private static ForumService CreateService(ApplicationDbContext context)
    {
        var uow = new UnitOfWork(context);
        return new ForumService(uow);
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public void CreateForum_NoGroup_ShouldPersistForum()
    {
        using var context = CreateContext();
        var service = CreateService(context);

        var result = service.CreateForum(new CreateForumDTO { Name = "Test" });

        Assert.True(result);
        Assert.Equal(1, context.Forums.Count());
        var forum = context.Forums.First();
        Assert.Equal("Test", forum.Name);
        Assert.Null(forum.Group);
    }

    [Fact]
    public void CreateForum_WithValidGroup_ShouldLinkForum()
    {
        using var context = CreateContext();
        var user = new User { Id = 1, Name = "owner", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var group = new Group { Id = 1, Name = "grp", Owner = user, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Users.Add(user);
        context.Groups.Add(group);
        context.SaveChanges();
        var service = CreateService(context);

        var result = service.CreateForum(new CreateForumDTO { Name = "Forum", GroupId = group.Id });

        Assert.True(result);
        Assert.Equal(1, context.Forums.Count());
        var forum = context.Forums.Include(f => f.Group).First();
        Assert.Equal(group.Id, forum.Group!.Id);
        Assert.Equal(forum.Id, context.Groups.First().Forum.Id);
    }

    [Fact]
    public void CreateForum_WithInvalidGroup_ShouldReturnFalse()
    {
        using var context = CreateContext();
        var service = CreateService(context);

        var result = service.CreateForum(new CreateForumDTO { Name = "Forum", GroupId = 99 });

        Assert.False(result);
        Assert.Empty(context.Forums);
    }
}
