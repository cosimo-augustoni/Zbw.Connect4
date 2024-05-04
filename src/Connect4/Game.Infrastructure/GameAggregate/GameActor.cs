using Game.Domain;
using Game.Domain.GameAggregate;
using Orleans.Providers;
using Shared.Domain;
using Shared.Infrastructure;

namespace Game.Infrastructure.GameAggregate
{
    [StorageProvider(ProviderName = "games")]
    [LogConsistencyProvider(ProviderName = "LogStorage")]
    public class GameActor : EventSourcedOrleansActor<Domain.GameAggregate.Game, GameState, GameEvent>, IGameActor
    {
        public GameActor(IEventPublisher eventPublisher)
            : base(eventPublisher, AggregateFactory)
        {
        }

        private static Domain.GameAggregate.Game AggregateFactory(IEventRegistry<GameEvent> eventRegistry, Guid id)
        {
            return new Domain.GameAggregate.Game(new GameId(id), eventRegistry);
        }

        public async Task<Guid> CreateGame()
        {
            return await this.ExecuteAsync(async (aggregate) => await aggregate.CreateGame());
        }

        public async Task ChangeNameAsync(string name)
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.ChangeNameAsync(name));
        }
    }
}