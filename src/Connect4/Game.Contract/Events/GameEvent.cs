using Shared.Contract;

namespace Game.Contract.Events
{
    public abstract record GameEvent : DomainEvent
    {
        public required GameId GameId { get; init; }
    }
}