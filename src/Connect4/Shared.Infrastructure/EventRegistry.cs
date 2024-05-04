using Shared.Contract;
using Shared.Domain;

namespace Shared.Infrastructure
{
    public class EventRegistry<TEventBase> : IEventRegistry<TEventBase> 
        where TEventBase : DomainEvent
    {
        public void RegisterEvent(TEventBase @event)
        {
            this.events.Add(@event);
        }

        private readonly List<TEventBase> events = [];

        public IReadOnlyList<TEventBase> Events => this.events.AsReadOnly();

        public void Clear()
        {
            this.events.Clear();
        }
    }
}