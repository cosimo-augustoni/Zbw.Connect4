namespace Shared.Domain
{
    public abstract class AggregateRoot<TEventBase>(IEventRegistry<TEventBase> eventRegistry)
        where TEventBase : DomainEvent
    {
        protected Task RaiseEventAsync(TEventBase @event)
        {
            eventRegistry.RegisterEvent(@event);

            return Task.CompletedTask;
        }
    }
}