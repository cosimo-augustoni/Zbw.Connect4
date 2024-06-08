using Game.Contract;
using Game.Contract.Events;
using Shared.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public record GamePiecePlacedEvent : GameEvent, IExternalDomainEvent<GamePiecePlacedEventDto>
    {
        public required Player PlacedBy { get; init; }
        public required PlayerSide PlayingSide { get; init; } 
        public required BoardPosition Position { get; init; }

        public GamePiecePlacedEventDto ToExternalDomainEvent()
        {
            return new GamePiecePlacedEventDto
            {
                GameId = this.GameId,
                PlacedBy = this.PlacedBy,
                PlayingSide = this.PlayingSide,
                Position = this.Position,
            };
        }
    }
}