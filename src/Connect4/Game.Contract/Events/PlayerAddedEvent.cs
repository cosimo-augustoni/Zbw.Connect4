namespace Game.Contract.Events
{
    public record PlayerAddedEvent : GameEvent
    {
        public required Player Player { get; init; }

        public required PlayerSide PlayerSide { get; init; }
    }
}