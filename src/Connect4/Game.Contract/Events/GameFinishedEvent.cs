namespace Game.Contract.Events
{
    public record GameFinishedEvent : GameEvent
    {
        public required FinishReason FinishReason { get; init; }

        public PlayerId? WinningPlayerId { get; init; }
    }
}