namespace Shared.Domain
{
    public interface IAggregateRoot<in TEventBase>
        where TEventBase : DomainEvent
    {
        void Apply(TEventBase @event);
    }
}