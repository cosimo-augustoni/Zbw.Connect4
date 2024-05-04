using Shared.Domain;

namespace Game.Domain.GameAggregate
{
    public abstract record GameEvent : DomainEvent
    {
        public required GameId GameId { get; init; }
    }
}