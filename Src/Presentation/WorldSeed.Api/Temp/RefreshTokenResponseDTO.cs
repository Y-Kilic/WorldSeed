using WorldSeed.Application.DTOS;

namespace WorldSeed.Api.Temp
{
    public class RefreshTokenResponseDTO
    {
        public TokenDTO Token { get; set; }
        public RefreshTokenDTO RefreshTokenDTO { get; set; }
    }
}
