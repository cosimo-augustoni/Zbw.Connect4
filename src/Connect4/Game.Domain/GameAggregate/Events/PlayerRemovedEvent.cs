using Game.Contract;
using Game.Contract.Events;
using Shared.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public record PlayerRemovedEvent : GameEvent, IExternalDomainEvent<PlayerRemovedEventDto>
    {
        public required PlayerId PlayerId { get; init; }

        public PlayerRemovedEventDto ToExternalDomainEvent()
        {
            return new PlayerRemovedEventDto
            {
                GameId = this.GameId,
                PlayerId = this.PlayerId,
            };
        }
    }
}