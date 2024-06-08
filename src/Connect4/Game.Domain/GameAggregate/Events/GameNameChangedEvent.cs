namespace Game.Domain.GameAggregate.Events
{
    public record GameNameChangedEvent : GameEvent
    {
        public required string Name { get; init; }
    }
}