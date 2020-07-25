using System.Threading.Tasks;
using Enexure.MicroBus;
using Strenua.Domain;

namespace Strenua.MicroBus
{
    public interface IDomainEventHandler<in TDomainEvent> : IMessageHandler<TDomainEvent, Unit> where TDomainEvent: IDomainEvent
    {
        
    }

    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        public virtual async Task<Unit> Handle(TDomainEvent message)
        {
            await HandleAsync(message);
            return Unit.Unit;
        }

        public abstract Task HandleAsync(TDomainEvent domainEvent);
    }
}