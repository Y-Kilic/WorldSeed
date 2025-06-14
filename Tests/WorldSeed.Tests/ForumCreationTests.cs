using Microsoft.EntityFrameworkCore;
using WorldSeed.Infrastructure.Data;
using WorldSeed.Persistence.Services;
using WorldSeed.Domain.Entities.GroupRelated;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Tests;

public class ForumCreationTests
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

    [Fact]
    public void CreateGroup_ShouldCreateForum()
    {
        using var context = CreateContext();
        var user = new User { Id = 1, Name = "owner", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Users.Add(user);
        context.SaveChanges();
        var service = CreateService(context);

        var group = service.CreateGroup("ForumGroup", user.Id);

        Assert.NotNull(group.Forum);
        Assert.Equal("ForumGroup Forum", group.Forum.Name);
        Assert.Equal(1, context.Forums.Count());
    }
}
