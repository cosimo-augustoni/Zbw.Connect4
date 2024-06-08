using Game.Contract;
using Shared.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public abstract record GameEvent : DomainEvent
    {
        public required GameId GameId { get; init; }
    }
}