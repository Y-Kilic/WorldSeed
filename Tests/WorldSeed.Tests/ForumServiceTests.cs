using Microsoft.EntityFrameworkCore;
using WorldSeed.Application.DTOS;
using WorldSeed.Infrastructure.Data;
using WorldSeed.Infrastructure.Repositories;
using WorldSeed.Persistence.Services;

namespace WorldSeed.Tests;

public class ForumServiceTests
{
    [Fact]
    public void CreateForum_AddsForumToDatabase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("CreateForum_AddsForumToDatabase")
            .Options;

        using var context = new ApplicationDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var service = new ForumService(unitOfWork);
        var dto = new CreateForumDTO { Name = "General" };

        var result = service.CreateForum(dto);

        Assert.True(result);
        Assert.Equal(1, context.Forums.Count());
        Assert.Equal("General", context.Forums.First().Name);
    }
}
