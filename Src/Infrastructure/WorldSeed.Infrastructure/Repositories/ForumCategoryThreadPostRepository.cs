using WorldSeed.Domain.Entities.ForumRelated;
using WorldSeed.Domain.Interfaces.Repositories;
using WorldSeed.Infrastructure.Data;

namespace WorldSeed.Infrastructure.Repositories
{
    public class ForumCategoryThreadPostRepository : Repository<ForumCategoryThreadPost>, IForumCategoryThreadPostRepository
    {
        public ForumCategoryThreadPostRepository(ApplicationDbContext context) : base(context)
        {
        }

        ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
