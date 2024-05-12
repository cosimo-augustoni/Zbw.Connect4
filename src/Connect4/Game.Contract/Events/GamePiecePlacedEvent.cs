namespace Game.Contract.Events
{
    public record GamePiecePlacedEvent : GameEvent
    {
        public PlayerSide PlayerSide { get; init; }
        public required BoardPosition Position { get; init; }
    }
}