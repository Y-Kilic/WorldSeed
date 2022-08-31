using WorldSeed.Application.DTOS;

namespace WorldSeed.Api.Temp
{
    public class LoginTokenResponseDTO
    {
        public string Token { get; set; }
        public RefreshTokenDTO RefreshTokenDTO { get; set; }
    }
}
