using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WorldSeed.Api.Temp;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Persistence.Services;
using WorldSeed.Domain.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using WorldSeed.Common.Validators;

namespace WorldSeed.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public UserController(IAccountService accountService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpPost("createUser")]
        public IActionResult CreateUser(CreateUserDTO createUserDTO)
        {
            UserValidator userValidator = new UserValidator();
            FluentValidation.Results.ValidationResult validationResult = userValidator.Validate(createUserDTO);

            if(!validationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var claim = Request.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "accountId");
            if (claim == null || !int.TryParse(claim.Value, out var currentAccountId))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var account = _accountService.GetAccountById(currentAccountId);
            if (account == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var newUser = _userService.CreateUser(account.Id, createUserDTO.UserName);

            if (newUser != null)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            else if (newUser == null) 
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }


            return StatusCode(StatusCodes.Status400BadRequest);

        }

        [Authorize]
        [HttpGet("getAccountUsers")]
        public List<GetAccountUsersResponseDTO> GetAccountUsers()
        {
            var claim = Request.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "accountId");
            if (claim == null || !int.TryParse(claim.Value, out var currentAccountId))
            {
                return new List<GetAccountUsersResponseDTO>();
            }

            var account = _accountService.GetAccountById(currentAccountId);
            if (account == null)
            {
                return new List<GetAccountUsersResponseDTO>();
            }

            var users = _userService.GetUsersbyAccountId(currentAccountId);

            var strippedUserList = new List<GetAccountUsersResponseDTO>();

            foreach (var user in users)
            {
                strippedUserList.Add(new GetAccountUsersResponseDTO() { Id = user.Id, Name = user.Name });
            }

            return strippedUserList;
        }
    }
}
