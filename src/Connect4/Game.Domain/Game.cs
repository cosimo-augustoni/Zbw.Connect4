using Shared.Domain;

namespace Game.Domain
{
    public class Game(GameId id, IEventRegistry<GameEvent> eventRegistry) : AggregateRoot<GameEvent>(eventRegistry), IGame
    {
        public GameId Id { get; } = id;

        public string Name { get; private set; } = "Connect 4 Game";

        public async Task<Guid> CreateGame()
        {
            await this.RaiseEventAsync(new GameCreatedEvent
            {
                GameId = this.Id
            });

            return this.Id.Id;
        }

        public async Task UpdateGameNameAsync(string name)
        {
            if(this.Name == name)
                return;

            await this.RaiseEventAsync(new GameNameChangedEvent
            {
                GameId = this.Id,
                Name = name
            });
        }

        public override void Apply(GameEvent @event)
        {
            switch (@event)
            {
                case GameCreatedEvent gameCreatedEvent:
                    this.Apply(gameCreatedEvent);
                    break;
                case GameNameChangedEvent gameNameChangedEvent:
                    this.Apply(gameNameChangedEvent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@event));
            }
        }

        private void Apply(GameNameChangedEvent @event)
        {
            this.Name = @event.Name;
        }

        private void Apply(GameCreatedEvent _)
        {
        }
    }

    public record GameCreatedEvent : GameEvent;

    public record GameNameChangedEvent : GameEvent
    {
        public required string Name { get; init; }
    }

    public abstract record GameEvent : DomainEvent
    {
        public required GameId GameId { get; init; }
    }
}
