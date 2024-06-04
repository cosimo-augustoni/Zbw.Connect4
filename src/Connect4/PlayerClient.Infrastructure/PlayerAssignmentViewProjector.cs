using Game.Contract.Events;
using MediatR;
using MongoDB.Driver;
using PlayerClient.Contract.Queries;

namespace PlayerClient.Infrastructure
{
    internal class PlayerAssignmentViewProjector(IMongoDatabase database, IPublisher notificationPublisher)
        : INotificationHandler<PlayerAddedEvent>, 
            INotificationHandler<PlayerRemovedEvent>
    {
        public async Task Handle(PlayerAddedEvent @event, CancellationToken cancellationToken)
        {
            var playerAssignmentViewDbo = new PlayerAssignmentViewDbo
            {
                GameId = @event.GameId.Id,
                PlayerId = @event.Player.Id.Id,
                PlayerType = @event.Player.TypeIdentifier
            };
            await this.GetPlayerAssignmentsCollection().InsertOneAsync(playerAssignmentViewDbo, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new PlayerClientCreatedNotification { GameId = @event.GameId, PlayerId = @event.Player.Id }, cancellationToken);
        }

        public async Task Handle(PlayerRemovedEvent @event, CancellationToken cancellationToken)
        {
            await this.GetPlayerAssignmentsCollection()
                .FindOneAndDeleteAsync(g => g.PlayerId == @event.PlayerId.Id, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new PlayerClientDeletedNotification { GameId = @event.GameId, PlayerId = @event.PlayerId }, cancellationToken);
        }

        private IMongoCollection<PlayerAssignmentViewDbo> GetPlayerAssignmentsCollection() => database.GetCollection<PlayerAssignmentViewDbo>("player_assignments");
    }
}
