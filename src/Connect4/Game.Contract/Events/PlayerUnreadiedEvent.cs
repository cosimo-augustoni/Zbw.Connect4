namespace Game.Contract.Events
{
    public record PlayerUnreadiedEvent : GameEvent
    {
        public required PlayerId PlayerId { get; init; }
    }
}