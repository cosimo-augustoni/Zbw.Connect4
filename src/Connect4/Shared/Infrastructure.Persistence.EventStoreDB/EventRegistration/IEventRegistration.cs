using Domain;

namespace Infrastructure.Persistence.EventStoreDB.EventRegistration
{
    internal interface IEventRegistration<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TAggregateRoot>
    {
        public string EventType { get; }

        public DomainEvent<TAggregateRoot> DeserializeJson(ReadOnlySpan<byte> eventData);
    }
}