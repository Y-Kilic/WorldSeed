using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IPasswordHasher _passwordHasher;

        public AuthController(IConfiguration configuration, IAccountService accountService, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _configuration = configuration;
            _accountService = accountService;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public ActionResult<string> Register(AccountRegisterDTO request)
        {
            AccountValidator accountValidator = new AccountValidator();
            FluentValidation.Results.ValidationResult validationResult = accountValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            _passwordHasher.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

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
        public ActionResult<LoginTokenResponseDTO> Login(AccountLoginRequestDTO request)
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
        public ActionResult<RefreshTokenResponseDTO> RefreshToken(RefreshTokenRequestDTO refreshTokenRequestDTO)
        {

            var claim = Request.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "accountId");
            if (claim == null || !int.TryParse(claim.Value, out var currentUserId))
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

    }
}
