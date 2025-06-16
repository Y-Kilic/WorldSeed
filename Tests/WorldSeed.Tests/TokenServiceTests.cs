using Microsoft.Extensions.Configuration;
using WorldSeed.Api.Temp;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Application.DTOS;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Tests;

public class TokenServiceTests
{
    private class StubAccountService : IAccountService
    {
        private readonly Account _account;

        public StubAccountService(Account account)
        {
            _account = account;
        }

        public Account GetAccountById(int accountId) => _account;
        public Account CreateAccount(string username, string email, byte[] passwordHash, byte[] passwordSalt) => throw new NotImplementedException();
        public Account CheckLoginByEmail(string email, string password) => throw new NotImplementedException();
        public Account GetAccountByEmail(string email) => throw new NotImplementedException();
        public Account GetAccountByUsername(string username) => throw new NotImplementedException();
        public void UpdateTokens(int accountId, string refreshToken, DateTime expires, DateTime created) => throw new NotImplementedException();
        public User? GetDefaultUser(int accountId) => _account.DefaultUser;
        public bool SetDefaultUser(int accountId, int userId) => throw new NotImplementedException();
    }

    private static TokenService CreateService(Account account)
    {
        var settings = new Dictionary<string, string> { { "AppSettings:Token", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" } };
        IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        var accSvc = new StubAccountService(account);
        return new TokenService(config, accSvc);
    }

    [Fact]
    public void CreateToken_ReturnsTokenDTO()
    {
        var service = CreateService(new Account());

        var token = service.CreateToken(1);

        Assert.False(string.IsNullOrWhiteSpace(token.Token));
        Assert.True(token.ValidTo > token.ValidFrom);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsValidDTO()
    {
        var service = CreateService(new Account());

        var refresh = service.GenerateRefreshToken();

        Assert.False(string.IsNullOrWhiteSpace(refresh.Token));
        Assert.True(refresh.Expires > DateTime.UtcNow);
    }

    [Fact]
    public void IsRefreshTokenValid_WithValidToken_ReturnsTrue()
    {
        var account = new Account
        {
            RefreshToken = "abc",
            TokenExpires = DateTime.UtcNow.AddMinutes(5)
        };
        var service = CreateService(account);

        Assert.True(service.IsRefreshTokenValid(1, "abc"));
    }

    [Fact]
    public void IsRefreshTokenValid_WithInvalidToken_ReturnsFalse()
    {
        var account = new Account
        {
            RefreshToken = "abc",
            TokenExpires = DateTime.UtcNow.AddMinutes(-1)
        };
        var service = CreateService(account);

        Assert.False(service.IsRefreshTokenValid(1, "abc"));
        Assert.False(service.IsRefreshTokenValid(1, "other"));
    }
}

