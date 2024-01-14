using Domain;

namespace Infrastructure.Persistence.EventStoreDB.EventRegistration
{
    internal class EventTransformer<TAggregateRoot>(IEnumerable<IEventRegistration<TAggregateRoot>> eventRegistrations)
        : IEventTransformer<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TAggregateRoot>
    {
        public DomainEvent<TAggregateRoot> Transform(string eventType, ReadOnlySpan<byte> eventData)
        {
            var eventRegistration = eventRegistrations.Single(r => r.EventType == eventType);
            return eventRegistration.DeserializeJson(eventData);
        }
    }
}