using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strenua.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
        IEnumerable<IDomainEvent> GetUncommittedEvents();
        void Rollback();
    }
}