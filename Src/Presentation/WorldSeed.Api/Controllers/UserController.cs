using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost("createUser")]
        public StatusCodeResult CreateUser(CreateUserDTO createUserDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var currentAccountId = int.Parse(claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value);



                var account = _accountService.GetAccountById(currentAccountId);

                var newUser = _userService.CreateUser(account.Id, createUserDTO.UserName);

                if (newUser != null)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }
            return StatusCode(StatusCodes.Status400BadRequest);

        }
    }
}
