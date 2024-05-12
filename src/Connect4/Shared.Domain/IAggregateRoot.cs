using Shared.Contract;

namespace Shared.Domain
{
    public interface IAggregateRoot<in TEventBase>
        where TEventBase : DomainEvent
    {
        public void Apply(TEventBase @event);
    }
}