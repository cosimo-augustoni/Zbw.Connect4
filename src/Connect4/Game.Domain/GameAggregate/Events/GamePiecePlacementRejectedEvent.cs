using Game.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public record GamePiecePlacementRejectedEvent : GameEvent
    {
        public required BoardPosition Position { get; init; }
    }
}