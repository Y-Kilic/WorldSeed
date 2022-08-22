using WorldSeed.Application.DTOS;
using WorldSeed.Domain.Entities.AccountRelated;
using WorldSeed.Persistence.Services;

namespace WorldSeed.Application.Interfaces.Services
{
    public interface IAccountService
    {
        public bool CreateAccount(CreateAccountDTO createAccountDTO);
        public Account CheckLogin(LoginAccountDTO loginAccountDTO);

    }
}