using Game.Contract;
using Game.Contract.Events;
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

        public async Task AddPlayer(Player player, PlayerSide playerSide)
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.AddPlayer(player, playerSide));
        }

        public async Task RemovePlayer(PlayerId playerId)
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.RemovePlayer(playerId));
        }

        public async Task StartGame()
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.StartGame());
        }

        public async Task ReadyPlayer(PlayerId playerId)
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.ReadyPlayer(playerId));
        }

        public async Task UnreadyPlayer(PlayerId playerId)
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.UnreadyPlayer(playerId));
        }

        public async Task PlaceGamePiece(BoardPosition boardPosition)
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.PlaceGamePiece(boardPosition));
        }

        public async Task AcknowledgeGamePiecePlacement(PlayerId playerId)
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.AcknowledgeGamePiecePlacement(playerId));
        }

        public async Task NotAcknowledgeGamePiecePlacement(PlayerId playerId)
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.NotAcknowledgeGamePiecePlacement(playerId));
        }

        public async Task AbortGame()
        {
            await this.ExecuteAsync(async (aggregate) => await aggregate.AbortGame());
        }

        public async Task<Board> GetBoardState()
        {
            return await this.ExecuteAsync(async (aggregate) => await aggregate.GetBoardState());
        }
    }
}