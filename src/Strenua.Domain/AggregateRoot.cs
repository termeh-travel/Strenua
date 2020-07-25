using System;
using System.Collections.Generic;
using System.Linq;

namespace Strenua.Domain
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<IDomainEvent> Events { get; }
        void ClearEvents();
    }

    public interface IAggregateRoot<TIdentifier> : IAggregateRoot, IEntity<TIdentifier> where TIdentifier : notnull
    {
        
    }

    public class AggregateRoot<TIdentifier> : Entity<TIdentifier>, IAggregateRoot<TIdentifier> where TIdentifier : notnull
    {
        private readonly Dictionary<Type, IDomainEvent> _events = new Dictionary<Type, IDomainEvent>();
        public IReadOnlyCollection<IDomainEvent> Events => _events.Values.ToList().AsReadOnly();

        public void ClearEvents() => _events.Clear();

        protected void AddEvent(IDomainEvent domainEvent)
        {
            _events[domainEvent.GetType()] = domainEvent;
        }
    }
}