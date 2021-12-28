using M.YZ.Basement.Core.Domain.Events;

namespace M.YZ.Basement.Core.Contracts.ApplicationServices.Events;
public interface IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent Event);
}

