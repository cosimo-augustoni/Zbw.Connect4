using Shared.Domain;

namespace Game.Domain.GameAggregate
{
    public class Game(GameId id, IEventRegistry<GameEvent> eventRegistry) : AggregateRoot<GameEvent>(eventRegistry), IGame
    {
        public Game(IEventRegistry<GameEvent> eventRegistry, GameId id, string? name) : this(id, eventRegistry)
        {
            this.Name = name;
        }

        public GameId Id { get; } = id;

        public string? Name { get; private set; }

        public async Task<Guid> CreateGame()
        {
            await this.RaiseEventAsync(new GameCreatedEvent
            {
                GameId = this.Id
            });

            await this.ChangeNameAsync("Connect 4 Game");

            return this.Id.Id;
        }

        public async Task ChangeNameAsync(string name)
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
                case GameCreatedEvent:
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
    }
}
