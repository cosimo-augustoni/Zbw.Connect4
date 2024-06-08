namespace Game.Contract.Events
{
    public record PlayerAddedEventDto : ExternalGameEvent
    {
        public required Player Player { get; init; }

        public required PlayerSide PlayerSide { get; init; }
    }
}