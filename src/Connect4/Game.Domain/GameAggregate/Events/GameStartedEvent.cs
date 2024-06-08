using Game.Contract;
using Game.Contract.Events;
using Shared.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public record GameStartedEvent : GameEvent, IExternalDomainEvent<GameStartedEventDto>
    {
        public required PlayerId StartingPlayerId { get; init; }
        public GameStartedEventDto ToExternalDomainEvent()
        {
            return new GameStartedEventDto
            {
                GameId = this.GameId,
                StartingPlayerId = this.StartingPlayerId,
            };
        }
    }
}