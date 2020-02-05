using System;
using System.Collections.Generic;
using System.Text;

namespace Windstorm.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
    }
}
