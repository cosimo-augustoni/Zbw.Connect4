namespace Game.Contract.Events
{
    public record GamePiecePlacementRequestedEvent : GameEvent
    {
        public required PlayerId RequestingPlayer { get; init; }
        public required BoardPosition Position { get; init; }
    }
}