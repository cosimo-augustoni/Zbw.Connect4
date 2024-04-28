using Game.Domain;
using Orleans.Providers;
using Shared.Domain;
using Shared.Infrastructure;

namespace Game.Infrastructure
{
    [StorageProvider(ProviderName = "games")]
    [LogConsistencyProvider(ProviderName = "LogStorage")]
    public class GameActor : EventSourcedOrleansActor<Domain.Game, GameState, GameEvent>, IGameActor
    {
        public GameActor(IEventPublisher eventPublisher)
            : base(eventPublisher, AggregateFactory)
        {
        }

        private static Domain.Game AggregateFactory(IEventRegistry<GameEvent> eventRegistry, Guid id)
        {
            return new Domain.Game(new GameId(id), eventRegistry);
        }

        public async Task<Guid> CreateGame()
        {
            return await this.ExecuteAsync(async (aggregate) => await aggregate.CreateGame());
        }

        public async Task UpdateGameNameAsync(string name)
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.UpdateGameNameAsync(name));
        }
    }
}