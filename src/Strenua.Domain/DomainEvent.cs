using System;

namespace Strenua.Domain
{
    public interface IDomainEvent
    {
        public Type AggregateRootType { get; }
        
        DateTime RaisedAt { get; }
    }
    
    public abstract class DomainEvent<TAggregateRoot> : IDomainEvent
    {
        public Type AggregateRootType => typeof(TAggregateRoot);

        public DateTime RaisedAt { get; } = DateTime.UtcNow;
    }
}