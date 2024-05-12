namespace Game.Contract.Events
{
    public record GamePiecePlacementRejectedEvent : GameEvent
    {
        public required BoardPosition Position { get; init; }
    }
}