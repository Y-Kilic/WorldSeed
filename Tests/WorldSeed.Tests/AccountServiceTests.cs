using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Infrastructure.Data;
using WorldSeed.Persistence.Services;

namespace WorldSeed.Tests;

public class AccountServiceTests
{
    private static AccountService CreateService(ApplicationDbContext context)
    {
        var uow = new UnitOfWork(context);
        return new AccountService(uow);
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    private static (byte[] hash, byte[] salt) Hash(string password)
    {
        using var hmac = new HMACSHA512();
        return (hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)), hmac.Key);
    }

    [Fact]
    public void CreateAccount_ShouldPersistAccount()
    {
        using var context = CreateContext();
        var service = CreateService(context);
        var (hash, salt) = Hash("pass");

        var account = service.CreateAccount("user", "user@example.com", hash, salt);

        Assert.NotNull(account);
        Assert.Equal(1, context.Accounts.Count());
    }

    [Fact]
    public void CheckLoginByEmail_WithValidCredentials_ReturnsAccount()
    {
        using var context = CreateContext();
        var service = CreateService(context);
        var (hash, salt) = Hash("pass");
        var account = service.CreateAccount("user", "user@example.com", hash, salt);

        var result = service.CheckLoginByEmail("user@example.com", "pass");

        Assert.NotNull(result);
        Assert.Equal(account.Id, result!.Id);
    }

    [Fact]
    public void GetAccountById_ReturnsAccount()
    {
        using var context = CreateContext();
        var service = CreateService(context);
        var (hash, salt) = Hash("pass");
        var account = service.CreateAccount("user", "user@example.com", hash, salt);

        var fetched = service.GetAccountById(account.Id);

        Assert.NotNull(fetched);
        Assert.Equal(account.UserName, fetched!.UserName);
    }

    [Fact]
    public void GetAccountByEmail_ReturnsAccount()
    {
        using var context = CreateContext();
        var service = CreateService(context);
        var (hash, salt) = Hash("pass");
        var account = service.CreateAccount("user", "user@example.com", hash, salt);

        var fetched = service.GetAccountByEmail("user@example.com");

        Assert.NotNull(fetched);
        Assert.Equal(account.Id, fetched!.Id);
    }

    [Fact]
    public void GetAccountByUsername_ReturnsAccount()
    {
        using var context = CreateContext();
        var service = CreateService(context);
        var (hash, salt) = Hash("pass");
        var account = service.CreateAccount("user", "user@example.com", hash, salt);

        var fetched = service.GetAccountByUsername("user");

        Assert.NotNull(fetched);
        Assert.Equal(account.Id, fetched!.Id);
    }

    [Fact]
    public void UpdateTokens_SetsRefreshTokenData()
    {
        using var context = CreateContext();
        var service = CreateService(context);
        var (hash, salt) = Hash("pass");
        var account = service.CreateAccount("user", "user@example.com", hash, salt);

        var expires = DateTime.UtcNow.AddHours(1);
        var created = DateTime.UtcNow;
        service.UpdateTokens(account.Id, "token", expires, created);

        var refreshed = context.Accounts.First();
        Assert.Equal("token", refreshed.RefreshToken);
        Assert.Equal(expires, refreshed.TokenExpires);
        Assert.Equal(created, refreshed.TokenCreated);
    }
}

