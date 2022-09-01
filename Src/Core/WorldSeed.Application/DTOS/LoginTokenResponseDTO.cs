using System;

namespace WorldSeed.Application.DTOS
{
    public class LoginTokenResponseDTO
    {
        public string Token { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public RefreshTokenDTO RefreshTokenDTO { get; set; }
    }
}
