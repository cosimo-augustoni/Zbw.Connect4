namespace Game.Domain.GameAggregate.Events
{
    public record GameCreatedEvent : GameEvent
    {
        public required string Name { get; init; }
    }
}