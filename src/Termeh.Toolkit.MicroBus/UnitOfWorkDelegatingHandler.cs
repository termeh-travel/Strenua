using System;
using System.Linq;
using System.Threading.Tasks;
using Enexure.MicroBus;
using Termeh.Toolkit.Domain;

namespace Termeh.Toolkit.MicroBus
{
    public class UnitOfWorkDelegatingHandler : IDelegatingHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMicroMediator _mediator;

        public UnitOfWorkDelegatingHandler(IUnitOfWork unitOfWork, IMicroMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<object> Handle(INextHandler next, object message)
        {
            var result = await next.Handle(message);
            
            if (message is ICommand)
            {
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    _unitOfWork.Rollback();
                    throw;
                }

                var events = _unitOfWork.GetUncommittedEvents();
                
                foreach (var domainEvent in events)
                {
                    await _mediator.PublishAsync(domainEvent);
                }
            }

            return result;
        }
    }
}