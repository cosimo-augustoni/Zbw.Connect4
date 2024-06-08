using Game.Contract;
using Game.Contract.Queries.Notifications;
using Game.Domain.GameAggregate.Events;
using MediatR;
using MongoDB.Driver;

namespace Game.Infrastructure.GameProjections.Lobbies
{
    internal class GameLobbyProjector(IMongoDatabase database, IPublisher notificationPublisher)
        : INotificationHandler<GameCreatedEvent>
            , INotificationHandler<GameNameChangedEvent>
            , INotificationHandler<PlayerAddedEvent>
            , INotificationHandler<PlayerRemovedEvent>
            , INotificationHandler<GameStartedEvent>
            , INotificationHandler<GameAbortedEvent>
    {
        public async Task Handle(GameCreatedEvent notification, CancellationToken cancellationToken)
        {
            var gameLobbyDbo = new GameLobbyDbo
            {
                GameId = notification.GameId.Id,
                Name = notification.Name,
                OpenPlayerSlots = 2
            };
            await this.GetGameLobbiesCollection().InsertOneAsync(gameLobbyDbo, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new GameLobbyCreatedNotification() { GameId = notification.GameId }, cancellationToken);
        }

        public async Task Handle(GameNameChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<GameLobbyDbo>.Update.Set(g => g.Name, notification.Name);
            await this.GetGameLobbiesCollection()
                .FindOneAndUpdateAsync(g => g.GameId == notification.GameId.Id, updateNameDefinition, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new GameLobbyUpdatedNotification() { GameId = notification.GameId }, cancellationToken);
        }

        public async Task Handle(PlayerAddedEvent notification, CancellationToken cancellationToken)
        {
            var gameLobby = await this.GetGameLobbiesCollection().FindAsync(g => g.GameId == notification.GameId.Id, cancellationToken: cancellationToken);
            var updateNameDefinition = Builders<GameLobbyDbo>.Update.Set(g => g.OpenPlayerSlots, gameLobby.First(cancellationToken).OpenPlayerSlots - 1);
            await this.GetGameLobbiesCollection()
                .FindOneAndUpdateAsync(g => g.GameId == notification.GameId.Id, updateNameDefinition, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new GameLobbyUpdatedNotification() { GameId = notification.GameId }, cancellationToken);
        }

        public async Task Handle(PlayerRemovedEvent notification, CancellationToken cancellationToken)
        {
            var gameLobby = await this.GetGameLobbiesCollection().FindAsync(g => g.GameId == notification.GameId.Id, cancellationToken: cancellationToken);
            var updateNameDefinition = Builders<GameLobbyDbo>.Update.Set(g => g.OpenPlayerSlots, gameLobby.First(cancellationToken).OpenPlayerSlots + 1);
            await this.GetGameLobbiesCollection()
                .FindOneAndUpdateAsync(g => g.GameId == notification.GameId.Id, updateNameDefinition, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new GameLobbyUpdatedNotification() { GameId = notification.GameId }, cancellationToken);
        }

        public async Task Handle(GameStartedEvent notification, CancellationToken cancellationToken)
        {
            await this.DeleteLobby(notification.GameId, cancellationToken);
        }

        public async Task Handle(GameAbortedEvent notification, CancellationToken cancellationToken)
        {
            await this.DeleteLobby(notification.GameId, cancellationToken);
        }

        private async Task DeleteLobby(GameId gameId, CancellationToken cancellationToken)
        {
            await this.GetGameLobbiesCollection()
                .FindOneAndDeleteAsync(g => g.GameId == gameId.Id, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new GameLobbyDeletedNotification() { GameId = gameId }, cancellationToken);
        }

        private IMongoCollection<GameLobbyDbo> GetGameLobbiesCollection() => database.GetCollection<GameLobbyDbo>("game_lobbies");
        
    }
}