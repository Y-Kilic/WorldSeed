using System;
using System.Collections.Generic;
using System.Text;

namespace WorldSeed.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
    }
}
