using WorldSeed.Application.DTOS;

namespace WorldSeed.Api.Temp
{
    public interface ITokenService
    {
        public TokenDTO CreateToken(string accountEmail);
        public RefreshTokenDTO GenerateRefreshToken();
        public bool IsRefreshTokenValid(string accountEmail, string refreshToken);


    }
}
