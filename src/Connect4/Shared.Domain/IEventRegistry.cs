using Shared.Contract;

namespace Shared.Domain
{
    public interface IEventRegistry<in TEventBase> where TEventBase : DomainEvent
    {
        void RegisterEvent(TEventBase @event);
    }
}
