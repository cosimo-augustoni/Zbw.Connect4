namespace Game.Contract.Events
{
    public record PlayerRemovedEventDto : ExternalGameEvent
    {
        public required PlayerId PlayerId { get; init; }    
    }
}