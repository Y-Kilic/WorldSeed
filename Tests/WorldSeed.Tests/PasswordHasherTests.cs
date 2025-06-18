using WorldSeed.Api.Temp;

namespace WorldSeed.Tests;

public class PasswordHasherTests
{
    [Fact]
    public void CreatePasswordHash_GeneratesDifferentSaltEachCall()
    {
        var hasher = new PasswordHasher();

        hasher.CreatePasswordHash("pass", out var hash1, out var salt1);
        hasher.CreatePasswordHash("pass", out var hash2, out var salt2);

        Assert.NotEqual(Convert.ToBase64String(salt1), Convert.ToBase64String(salt2));
        Assert.NotEqual(Convert.ToBase64String(hash1), Convert.ToBase64String(hash2));
    }

    [Fact]
    public void CreatePasswordHash_CanBeVerifiedWithSameSalt()
    {
        var hasher = new PasswordHasher();

        hasher.CreatePasswordHash("pass", out var hash, out var salt);

        using var hmac = new System.Security.Cryptography.HMACSHA512(salt);
        var expected = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("pass"));

        Assert.Equal(expected, hash);
    }
}
