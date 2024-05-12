namespace Game.Contract.Events
{
    public record PlayerReadiedEvent : GameEvent
    {
        public required PlayerId PlayerId { get; init; }
    }
}