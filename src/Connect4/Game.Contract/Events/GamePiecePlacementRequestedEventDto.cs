namespace Game.Contract.Events
{
    public record GamePiecePlacementRequestedEventDto : ExternalGameEvent
    {
        public required Player RequestingPlayer { get; init; }
        public required BoardPosition Position { get; init; }
    }
}