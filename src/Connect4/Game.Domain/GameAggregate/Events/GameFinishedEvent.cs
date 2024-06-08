using Game.Contract;
using Game.Contract.Events;
using Shared.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public record GameFinishedEvent : GameEvent, IExternalDomainEvent<GameFinishedEventDto>
    {
        public required FinishReason FinishReason { get; init; }

        public PlayerId? WinningPlayerId { get; init; }

        public GameFinishedEventDto ToExternalDomainEvent()
        {
            return new GameFinishedEventDto
            {
                GameId = this.GameId,
                FinishReason = this.FinishReason,
                WinningPlayerId = this.WinningPlayerId
            };
        }
    }
}