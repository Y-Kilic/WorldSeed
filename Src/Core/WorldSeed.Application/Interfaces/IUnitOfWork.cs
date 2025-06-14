using System;
using WorldSeed.Application.Interfaces.Repositories;
using WorldSeed.Domain.Interfaces.Repositories;

namespace WorldSeed.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        IGroupRepository Groups { get; }
        IGroupMemberRepository GroupMembers { get; }
        IUserRepository Users { get; }


        int SaveChanges();
    }
}