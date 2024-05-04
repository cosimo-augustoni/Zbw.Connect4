using Game.Contract.Events;
using MediatR;
using MongoDB.Driver;

namespace Game.Infrastructure.GameProjections
{
    internal class GameSummaryProjector(IMongoDatabase database) : INotificationHandler<GameCreatedEvent>, INotificationHandler<GameNameChangedEvent>
    {
        public async Task Handle(GameCreatedEvent notification, CancellationToken cancellationToken)
        {
            var gameSummaryDbo = new GameSummaryDbo
            {
                GameId = notification.GameId.Id
            };
            await this.GetGameSummariesCollection().InsertOneAsync(gameSummaryDbo, cancellationToken: cancellationToken);
        }

        public async Task Handle(GameNameChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<GameSummaryDbo>.Update.Set(g => g.Name, notification.Name);
            await this.GetGameSummariesCollection()
                .FindOneAndUpdateAsync(g => g.GameId == notification.GameId.Id, updateNameDefinition, cancellationToken: cancellationToken);
        }

        private IMongoCollection<GameSummaryDbo> GetGameSummariesCollection() => database.GetCollection<GameSummaryDbo>("game_summaries");
    }
}