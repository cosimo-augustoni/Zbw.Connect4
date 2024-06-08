using Game.Contract;
using Game.Contract.Events;
using Shared.Contract;

namespace Game.Domain.GameAggregate.Events
{
    public record PlayerAddedEvent : GameEvent, IExternalDomainEvent<PlayerAddedEventDto>
    {
        public required Player Player { get; init; }

        public required PlayerSide PlayerSide { get; init; }

        public PlayerAddedEventDto ToExternalDomainEvent()
        {
            return new PlayerAddedEventDto
            {
                GameId = this.GameId,
                Player = this.Player,
                PlayerSide = this.PlayerSide,
            };
        }
    }
}