using Domain;

namespace Infrastructure.Persistence.EventStoreDB.EventRegistration
{
    internal interface IEventTransformer<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TAggregateRoot>
    {
        DomainEvent<TAggregateRoot> Transform(string eventType, ReadOnlySpan<byte> eventData);
    }
}