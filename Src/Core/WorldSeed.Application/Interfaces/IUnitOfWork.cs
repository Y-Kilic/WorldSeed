using System;
using WorldSeed.Application.Interfaces.Repositories;

namespace WorldSeed.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }

        int SaveChanges();
    }
}