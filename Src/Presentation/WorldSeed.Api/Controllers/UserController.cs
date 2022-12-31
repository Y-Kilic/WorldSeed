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
        public object CreateUser(CreateUserDTO createUserDTO)
        {
            UserValidator userValidator = new UserValidator();
            FluentValidation.Results.ValidationResult validationResult = userValidator.Validate(createUserDTO);

            if(!validationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var currentAccountId = int.Parse(Request.HttpContext.User.Claims.Where(c => c.Type == "accountId").FirstOrDefault().Value);
            
            var account = _accountService.GetAccountById(currentAccountId);

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
            var currentAccountId = int.Parse(Request.HttpContext.User.Claims.Where(c => c.Type == "accountId").FirstOrDefault().Value);

            var account = _accountService.GetAccountById(currentAccountId);

            var users = _userService.GetUsersbyAccountId(currentAccountId);

            if (users.Count > 0)
            {
                var StrippedUserList = new List<GetAccountUsersResponseDTO>();

                foreach (var user in users)
                {
                    StrippedUserList.Add(new GetAccountUsersResponseDTO() { Id = user.Id, Name = user.Name });
                }

                return StrippedUserList;
            }
            else
            {
                return null;
            }
        }
    }
}
