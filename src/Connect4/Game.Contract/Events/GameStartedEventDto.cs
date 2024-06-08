namespace Game.Contract.Events
{
    public record GameStartedEventDto : ExternalGameEvent
    {
        public required PlayerId StartingPlayerId { get; init; }
    }
}