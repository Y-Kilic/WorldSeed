using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WorldSeed.Api.Temp;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Common.Validators;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Persistence.Services;

namespace WorldSeed.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AuthController(IConfiguration configuration, IAccountService accountService, ITokenService tokenService)
        {
            _configuration = configuration;
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(AccountRegisterDTO request)
        {
            AccountValidator accountValidator = new AccountValidator();
            FluentValidation.Results.ValidationResult validationResult = accountValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var resultCreate =_accountService.CreateAccount(request.Username, request.Email, passwordHash, passwordSalt);

            if (resultCreate != null)
            {
                return Ok();
            }
            else
            {
                return Conflict("Account already exist.");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<RefreshTokenResponseDTO>> Login(AccountLoginRequestDTO request)
        {

            var result = _accountService.CheckLoginByEmail(request.Email, request.Password);

            if (result == null)
            {
                return BadRequest("Login not valid.");
            }

            var tokenDTO = _tokenService.CreateToken(result.Id);
            var refreshTokenDTO = _tokenService.GenerateRefreshToken();

            _accountService.UpdateTokens(
                result.Id,
                refreshTokenDTO.Token,
                refreshTokenDTO.Expires,
                refreshTokenDTO.Created
                );

            var AccountLoginResponseDTO = new LoginTokenResponseDTO()
            {
                Token = tokenDTO.Token,
                ValidFrom = tokenDTO.ValidFrom,
                ValidTo = tokenDTO.ValidTo,
                RefreshTokenDTO = refreshTokenDTO
            };

            return Ok(AccountLoginResponseDTO);
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<RefreshTokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO refreshTokenRequestDTO)
        {

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);

            if (currentUserId == null)
            {
                return BadRequest("Refreshtoken not valid.");

            }
            if (!_tokenService.IsRefreshTokenValid(currentUserId, refreshTokenRequestDTO.RefreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }

            var tokenDTO = _tokenService.CreateToken(currentUserId);
            var refreshTokenDTO = _tokenService.GenerateRefreshToken();

            _accountService.UpdateTokens(
                currentUserId,
                refreshTokenDTO.Token,
                refreshTokenDTO.Expires,
                refreshTokenDTO.Created
                );

            var refreshTokenResponseDTO = new RefreshTokenResponseDTO()
            {
                Token = tokenDTO,
                RefreshTokenDTO = refreshTokenDTO
            };

            return Ok(refreshTokenResponseDTO);
        }

        // TODO: Move this out of AuthController
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
