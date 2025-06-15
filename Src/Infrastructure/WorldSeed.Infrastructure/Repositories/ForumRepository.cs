using WorldSeed.Domain.Entities.ForumRelated;
using WorldSeed.Application.Interfaces.Repositories;
using WorldSeed.Infrastructure.Data;

namespace WorldSeed.Infrastructure.Repositories
{
    public class ForumRepository : Repository<Forum>, IForumRepository
    {
        public ForumRepository(ApplicationDbContext context) : base(context)
        {
        }

        ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
