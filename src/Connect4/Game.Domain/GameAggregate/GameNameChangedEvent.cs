namespace Game.Domain.GameAggregate
{
    public record GameNameChangedEvent : GameEvent
    {
        public required string Name { get; init; }
    }
}