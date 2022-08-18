using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorldSeed.Domain.Entities.CharacterRelated;
using WorldSeed.Domain.Entities.ForumRelated;
using WorldSeed.Domain.Entities.GroupRelated;
using WorldSeed.Domain.Entities.UserRelated;

namespace WorldSeed.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }

        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterMessage> CharacterMessages { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<ForumCategoryThread> ForumCategoriesThreads { get; set; }
        public DbSet<ForumCategoryThreadPost> ForumCategoryThreadPosts { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
