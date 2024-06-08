namespace Game.Contract.Events
{
    public record GameFinishedEventDto : ExternalGameEvent
    {
        public required FinishReason FinishReason { get; init; }

        public PlayerId? WinningPlayerId { get; init; }
    }
}