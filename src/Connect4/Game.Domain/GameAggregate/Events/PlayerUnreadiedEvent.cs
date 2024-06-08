using Game.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public record PlayerUnreadiedEvent : GameEvent
    {
        public required PlayerId PlayerId { get; init; }
    }
}