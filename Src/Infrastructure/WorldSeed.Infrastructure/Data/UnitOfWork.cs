﻿using System;
using System.Collections.Generic;
using System.Text;
using WorldSeed.Application.Interfaces;
using WorldSeed.Application.Interfaces.Repositories;
using WorldSeed.Domain.Interfaces.Repositories;
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
            Users = new UserRepository(_context);


        }

        public IAccountRepository Accounts { get; private set; }
        public IGroupRepository Groups { get; private set; }
        public IUserRepository Users { get; private set; }

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
