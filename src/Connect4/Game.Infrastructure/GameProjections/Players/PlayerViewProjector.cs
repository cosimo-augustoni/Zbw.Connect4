using Game.Contract.Events;
using Game.Contract.Queries.Notifications;
using MongoDB.Driver;
using MediatR;

namespace Game.Infrastructure.GameProjections.Players
{
    internal class PlayerViewProjector(IMongoDatabase database, IPublisher notificationPublisher)
        : INotificationHandler<PlayerReadiedEvent>,
        INotificationHandler<PlayerUnreadiedEvent>,
        INotificationHandler<PlayerAddedEvent>,
        INotificationHandler<PlayerRemovedEvent>
    {
        public async Task Handle(PlayerReadiedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = Builders<PlayerViewDbo>.Update.Set(g => g.IsReady, true);

            await this.GetPlayersCollection()
                .FindOneAndUpdateAsync(g => g.PlayerId == notification.PlayerId.Id, updateDefinition, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new PlayerUpdatedNotification { PlayerId = notification.PlayerId, GameId = notification.GameId}, cancellationToken);
        }

        public async Task Handle(PlayerUnreadiedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = Builders<PlayerViewDbo>.Update.Set(g => g.IsReady, false);

            await this.GetPlayersCollection()
                .FindOneAndUpdateAsync(g => g.PlayerId == notification.PlayerId.Id, updateDefinition, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new PlayerUpdatedNotification { PlayerId = notification.PlayerId, GameId = notification.GameId}, cancellationToken);
        }

        public async Task Handle(PlayerAddedEvent notification, CancellationToken cancellationToken)
        {
            var playerView = new PlayerViewDbo
            {
                PlayerId = notification.Player.Id.Id,
                Name = notification.Player.Name,
                IsReady = false
            };
            await this.GetPlayersCollection().InsertOneAsync(playerView, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new PlayerAddedNotification { GameId = notification.GameId }, cancellationToken);
        }

        public async Task Handle(PlayerRemovedEvent notification, CancellationToken cancellationToken)
        {
            await this.GetPlayersCollection()
                .FindOneAndDeleteAsync(g => g.PlayerId == notification.PlayerId.Id, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new PlayerRemovedNotification { GameId = notification.GameId }, cancellationToken);
        }

        private IMongoCollection<PlayerViewDbo> GetPlayersCollection() => database.GetCollection<PlayerViewDbo>("players");
    }
}
