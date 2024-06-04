namespace Game.Contract.Events
{
    public record GamePiecePlacementRequestedEvent : GameEvent
    {
        public required Player RequestingPlayer { get; init; }
        public required BoardPosition Position { get; init; }
    }
}