using Microsoft.EntityFrameworkCore;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Domain.Entities.UserRelated;
using WorldSeed.Infrastructure.Data;
using WorldSeed.Persistence.Services;

namespace WorldSeed.Tests;

public class UserServiceTests
{
    private static UserService CreateService(ApplicationDbContext context)
    {
        var uow = new UnitOfWork(context);
        return new UserService(uow);
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public void CreateUser_ShouldPersistUser()
    {
        using var context = CreateContext();
        var account = new Account { Id = 1, UserName = "user", Email = "e" };
        context.Accounts.Add(account);
        context.SaveChanges();
        var service = CreateService(context);

        var user = service.CreateUser(account.Id, "name");

        Assert.NotNull(user);
        Assert.Equal(1, context.Users.Count());
        Assert.Equal(account.Id, context.Users.First().Account.Id);
    }

    [Fact]
    public void CreateUser_DuplicateName_ReturnsNull()
    {
        using var context = CreateContext();
        var account = new Account { Id = 1, UserName = "user", Email = "e" };
        context.Accounts.Add(account);
        context.Users.Add(new User { Account = account, Name = "name", CreatedAt = DateTime.UtcNow });
        context.SaveChanges();
        var service = CreateService(context);

        var user = service.CreateUser(account.Id, "name");

        Assert.Null(user);
    }

    [Fact]
    public void GetUsersbyAccountId_ReturnsUsers()
    {
        using var context = CreateContext();
        var acc1 = new Account { Id = 1, UserName = "a1", Email = "a1" };
        var acc2 = new Account { Id = 2, UserName = "a2", Email = "a2" };
        context.Accounts.AddRange(acc1, acc2);
        context.Users.AddRange(
            new User { Account = acc1, Name = "u1", CreatedAt = DateTime.UtcNow },
            new User { Account = acc1, Name = "u2", CreatedAt = DateTime.UtcNow },
            new User { Account = acc2, Name = "u3", CreatedAt = DateTime.UtcNow }
        );
        context.SaveChanges();
        var service = CreateService(context);

        var result = service.GetUsersbyAccountId(acc1.Id);

        Assert.Equal(2, result.Count);
    }
}

