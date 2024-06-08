using Game.Contract;
using Game.Contract.Events;
using Shared.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public record GamePiecePlacementRequestedEvent : GameEvent, IExternalDomainEvent<GamePiecePlacementRequestedEventDto>
    {
        public required Player RequestingPlayer { get; init; }
        public required BoardPosition Position { get; init; }

        public GamePiecePlacementRequestedEventDto ToExternalDomainEvent()
        {
            return new GamePiecePlacementRequestedEventDto
            {
                GameId = this.GameId,
                RequestingPlayer = this.RequestingPlayer,
                Position = this.Position,
            };
        }
    }
}