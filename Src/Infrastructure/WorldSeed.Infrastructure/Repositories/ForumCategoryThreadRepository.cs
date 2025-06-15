using WorldSeed.Domain.Entities.ForumRelated;
using WorldSeed.Domain.Interfaces.Repositories;
using WorldSeed.Infrastructure.Data;

namespace WorldSeed.Infrastructure.Repositories
{
    public class ForumCategoryThreadRepository : Repository<ForumCategoryThread>, IForumCategoryThreadRepository
    {
        public ForumCategoryThreadRepository(ApplicationDbContext context) : base(context)
        {
        }

        ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
