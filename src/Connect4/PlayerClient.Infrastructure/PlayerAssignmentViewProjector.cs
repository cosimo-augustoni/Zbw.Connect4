using Game.Contract.Events;
using Game.Contract.Queries.Notifications;
using MediatR;
using MongoDB.Driver;

namespace PlayerClient.Infrastructure
{
    internal class PlayerAssignmentViewProjector(IMongoDatabase database)
        : INotificationHandler<PlayerAddedEvent>, 
            INotificationHandler<PlayerRemovedEvent>
    {
        public async Task Handle(PlayerAddedEvent @event, CancellationToken cancellationToken)
        {
            var playerAssignmentViewDbo = new PlayerAssignmentViewDbo
            {
                GameId = @event.GameId.Id,
                PlayerId = @event.Player.Id.Id
            };
            await this.GetPlayerAssignmentsCollection().InsertOneAsync(playerAssignmentViewDbo, cancellationToken: cancellationToken);
        }

        public async Task Handle(PlayerRemovedEvent @event, CancellationToken cancellationToken)
        {
            await this.GetPlayerAssignmentsCollection()
                .FindOneAndDeleteAsync(g => g.PlayerId == @event.PlayerId.Id, cancellationToken: cancellationToken);
        }

        private IMongoCollection<PlayerAssignmentViewDbo> GetPlayerAssignmentsCollection() => database.GetCollection<PlayerAssignmentViewDbo>("player_assignments");
    }
}
