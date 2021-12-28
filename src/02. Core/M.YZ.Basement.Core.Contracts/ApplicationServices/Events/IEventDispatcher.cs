using M.YZ.Basement.Core.Domain.Events;

namespace M.YZ.Basement.Core.ApplicationServices.Events;
public interface IEventDispatcher
{
    Task PublishDomainEventAsync<TDomainEvent>(TDomainEvent @event) where TDomainEvent : class, IDomainEvent;

}

