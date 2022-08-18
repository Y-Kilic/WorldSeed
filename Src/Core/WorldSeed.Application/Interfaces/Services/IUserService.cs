using WorldSeed.Application.DTOS;
using WorldSeed.Domain.Entities.User;
using WorldSeed.Persistence.Services;

namespace WorldSeed.Application.Interfaces.Services
{
    public interface IUserService
    {
        public bool CreateUser(CreateUserDTO createUserDTO);
        public User CheckLogin(LoginUserDTO loginUserDTO);

    }
}