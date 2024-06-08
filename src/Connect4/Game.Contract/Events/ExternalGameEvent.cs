using Shared.Contract;

namespace Game.Contract.Events
{
    public abstract record ExternalGameEvent : ExternalDomainEvent
    {
        public required GameId GameId { get; init; }
    }
}