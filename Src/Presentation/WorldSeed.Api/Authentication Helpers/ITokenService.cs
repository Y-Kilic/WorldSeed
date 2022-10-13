using WorldSeed.Application.DTOS;

namespace WorldSeed.Api.Temp
{
    public interface ITokenService
    {
        public TokenDTO CreateToken(int accountId);
        public RefreshTokenDTO GenerateRefreshToken();
        public bool IsRefreshTokenValid(int accountId, string refreshToken);


    }
}
