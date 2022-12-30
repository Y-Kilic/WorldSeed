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
    }
}
