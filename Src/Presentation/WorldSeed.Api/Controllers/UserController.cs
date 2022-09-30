using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldSeed.Application.DTOS;
using WorldSeed.Application.Interfaces.Services;

namespace WorldSeed.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        [HttpPost("createUser")]
        public StatusCodeResult CreateUser(CreateUserDTO createUserDTO)
        {
            _userService.CreateUser(createUserDTO);

            if (_userService.CreateUser(createUserDTO))
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
