using Shared.Contract;

namespace Shared.Domain
{
    public abstract class AggregateRoot<TEventBase>(IEventRegistry<TEventBase> eventRegistry) : IAggregateRoot<TEventBase>
        where TEventBase : DomainEvent
    {
        protected Task RaiseEventAsync(TEventBase @event)
        {
            eventRegistry.RegisterEvent(@event);

            return Task.CompletedTask;
        }

        public abstract void Apply(TEventBase @event);
    }
}