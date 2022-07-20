using WorldSeed.API.Data;
using WorldSeed.Domain.Entities;

namespace WorldSeed.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public ApplicationDbContext ApplicationDbContext { get; }
    }
}