﻿using System;
using System.Collections.Generic;
using System.Text;
using WorldSeed.Application.Interfaces;
using WorldSeed.Application.Interfaces.Repositories;
using WorldSeed.Infrastructure;
using WorldSeed.Infrastructure.Repositories;

namespace WorldSeed.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WorldSeed.Infrastructure.ApplicationDbContext _context;

        public UnitOfWork(WorldSeed.Infrastructure.ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }

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
