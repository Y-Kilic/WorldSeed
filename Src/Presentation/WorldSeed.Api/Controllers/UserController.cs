using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorldSeed.Api.Temp;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Persistence.Services;

namespace WorldSeed.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;


        public UserController(IAccountService accountService, IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }

        [HttpPost("createUser")]
        public StatusCodeResult CreateUser(CreateUserDTO createUserDTO)
        {
            var currentAccountId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = _accountService.GetAccountById(currentAccountId);

            var newUser = _userService.CreateUserAndSetAccountDefault(account.Id, createUserDTO.UserName);

            if (newUser != null)
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
