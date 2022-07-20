using System;
using WorldSeed.Application.Interfaces.Repositories;

namespace WorldSeed.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        int SaveChanges();
    }
}