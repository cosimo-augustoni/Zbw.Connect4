using Game.Contract.Events;
using Shared.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public record GameAbortedEvent : GameEvent, IExternalDomainEvent<GameAbortedEventDto>
    {
        public GameAbortedEventDto ToExternalDomainEvent()
        {
            return new GameAbortedEventDto
            {
                GameId = this.GameId,
            };
        }
    }
}