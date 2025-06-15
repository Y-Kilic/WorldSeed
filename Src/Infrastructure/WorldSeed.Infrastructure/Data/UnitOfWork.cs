using System;
using System.Collections.Generic;
using System.Text;
using WorldSeed.Application.Interfaces;
using WorldSeed.Application.Interfaces.Repositories;
using WorldSeed.Infrastructure.Repositories;

namespace WorldSeed.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Accounts = new AccountRepository(_context);
            Groups = new GroupRepository(_context);
            GroupMembers = new GroupMemberRepository(_context);
            Users = new UserRepository(_context);
            Forums = new ForumRepository(_context);
            ForumCategories = new ForumCategoryRepository(_context);
            ForumCategoryThreads = new ForumCategoryThreadRepository(_context);
            ForumCategoryThreadPosts = new ForumCategoryThreadPostRepository(_context);
        }

        public IAccountRepository Accounts { get; private set; }
        public IGroupRepository Groups { get; private set; }
        public IGroupMemberRepository GroupMembers { get; private set; }
        public IUserRepository Users { get; private set; }
        public IForumRepository Forums { get; private set; }
        public IForumCategoryRepository ForumCategories { get; private set; }
        public IForumCategoryThreadRepository ForumCategoryThreads { get; private set; }
        public IForumCategoryThreadPostRepository ForumCategoryThreadPosts { get; private set; }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
