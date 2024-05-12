namespace Game.Contract.Events
{
    public record PlayerRemovedEvent : GameEvent
    {
        public required PlayerId PlayerId { get; init; }
    }
}