using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Domain.Entities.AccountRelated;

namespace WorldSeed.Api.Temp
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;


        public TokenService(IConfiguration configuration, IAccountService accountService)
        {
            _configuration = configuration;
            _accountService = accountService;
        }
        public TokenDTO CreateToken(string accountId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, accountId)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            var tokenDTO = new TokenDTO()
            {
                Token = jwt,
                ValidFrom = token.ValidFrom,
                ValidTo = token.ValidTo
            };

            return tokenDTO;
        }
        public RefreshTokenDTO GenerateRefreshToken()
        {
            var refreshTokenDTO = new RefreshTokenDTO
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };

            return refreshTokenDTO;
        }

        public bool IsRefreshTokenValid(string accountId, string refreshToken)
        {
            var account = _accountService.GetAccountById(accountId);

            if (account.RefreshToken == refreshToken)
            {
                if(account.TokenExpires > DateTime.UtcNow)
                {
                    return true;
                }
            }

            return false;
        }
    }
}