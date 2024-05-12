namespace Game.Contract.Events
{
    public record GameCreatedEvent : GameEvent
    {
        public required string Name { get; init; }
    }
}