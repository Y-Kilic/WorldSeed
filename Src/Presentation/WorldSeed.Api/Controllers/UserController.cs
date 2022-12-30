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

            var testList = new List<String>();

            var obj = _httpContextAccessor.HttpContext;
            var currentAccountId = _httpContextAccessor.HttpContext
                .User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            testList.Add(currentAccountId ?? "null");

            ClaimsPrincipal claimIdentity = Request.HttpContext.User;
            testList.Add(claimIdentity.Claims.Count().ToString());
            return claimIdentity.Claims.ToList();

            var principal_name = Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"].FirstOrDefault();
            var principal_Id = Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].FirstOrDefault();

            testList.Add(principal_name ?? "principal_name is null");
            testList.Add(principal_Id ?? "principal_Id is null");

            var userIDToken = Request.Headers["X-MS-TOKEN-AAD-ID-TOKEN"];

            testList.Add("userIDToken " + userIDToken);


            return testList;
            Console.WriteLine("Account id: " + currentAccountId);

            var account = _accountService.GetAccountById(int.Parse(currentAccountId));

            var newUser = _userService.CreateUser(account.Id, createUserDTO.UserName);

            if (newUser != null)
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            return StatusCode(StatusCodes.Status400BadRequest);

        }
    }
}
