using Game.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public record PlayerReadiedEvent : GameEvent
    {
        public required PlayerId PlayerId { get; init; }
    }
}