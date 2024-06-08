namespace Game.Contract.Events
{
    public record GamePiecePlacedEventDto : ExternalGameEvent
    {
        public required Player PlacedBy { get; init; }
        public required PlayerSide PlayingSide { get; init; }
        public required BoardPosition Position { get; init; }
    }
}
