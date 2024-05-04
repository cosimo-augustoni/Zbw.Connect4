namespace Game.Contract.Events
{
    public record GameNameChangedEvent : GameEvent
    {
        public required string Name { get; init; }
    }
}