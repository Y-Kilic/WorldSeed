using WorldSeed.Application.DTOS;

namespace WorldSeed.Api.Temp
{
    public interface ITokenService
    {
        public TokenDTO CreateToken(string accountId);
        public RefreshTokenDTO GenerateRefreshToken();
        public bool IsRefreshTokenValid(string accountId, string refreshToken);


    }
}
