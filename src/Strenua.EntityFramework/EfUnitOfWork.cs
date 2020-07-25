using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Strenua.Domain;

namespace Strenua.EntityFramework
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public EfUnitOfWork(IDbContextAccessor dbContextAccessor)
        {
            _dbContext = dbContextAccessor.Context;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public IEnumerable<IDomainEvent> GetUncommittedEvents()
        {
            var aggregateRoots = _dbContext.ChangeTracker
                .Entries()
                .Where(x => x.Entity is IAggregateRoot)
                .Select(x => x.Entity as IAggregateRoot)
                .ToList();

            var domainEvents = aggregateRoots
                .Where(x => x.Events.Any())
                .SelectMany(x => x.Events)
                .ToList();
            
            foreach (var entity in aggregateRoots)
            {
                entity.ClearEvents();
            }

            return domainEvents;
        }

        public void Rollback()
        {
            
        }
    }
}