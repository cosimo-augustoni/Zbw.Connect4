using System.Runtime.Serialization;
using System.Text.Json;
using Domain;

namespace Infrastructure.Persistence.EventStoreDB.EventRegistration
{
    internal record class EventRegistration<TAggregateRoot, TClrType>(string EventType) : IEventRegistration<TAggregateRoot>
        where TClrType : DomainEvent<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TAggregateRoot>
    {
        public DomainEvent<TAggregateRoot> DeserializeJson(ReadOnlySpan<byte> eventData)
        {
            return JsonSerializer.Deserialize<TClrType>(eventData) ?? throw new SerializationException($"Event of Type: '{this.EventType}' cannot be deserialized");
        }
    }
}