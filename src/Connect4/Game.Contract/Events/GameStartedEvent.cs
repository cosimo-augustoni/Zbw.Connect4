namespace Game.Contract.Events
{
    public record GameStartedEvent : GameEvent
    {
        public required PlayerId StartingPlayerId { get; init; }
    }
}