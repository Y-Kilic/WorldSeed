using WorldSeed.Domain.Entities.ForumRelated;
using WorldSeed.Domain.Interfaces.Repositories;
using WorldSeed.Infrastructure.Data;

namespace WorldSeed.Infrastructure.Repositories
{
    public class ForumCategoryRepository : Repository<ForumCategory>, IForumCategoryRepository
    {
        public ForumCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
