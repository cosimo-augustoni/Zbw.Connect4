using MediatR;
using MongoDB.Driver;
using Visualizer.Contract.Events;
using Visualizer.Contract.Queries;

namespace Visualizer.Infrastructure.VisualizerProjections
{
    internal class VisualizerProjector(IVisualizerCollectionProvider collectionProvider, IPublisher notificationPublisher)
        : INotificationHandler<VisualizerCreatedEvent>
            , INotificationHandler<VisualizerNameChangedEvent>
            , INotificationHandler<VisualizerExternalIdChangedEvent>
            , INotificationHandler<VisualizerStatusChangedEvent>
            , INotificationHandler<VisualizerDeletedEvent>
    {
        public async Task Handle(VisualizerCreatedEvent notification, CancellationToken cancellationToken)
        {
            var visualizerDetailDbo = new VisualizerViewDbo
            {
                VisualizerId = notification.VisualizerId.Id,
            };
            await collectionProvider.VisualizerDetailCollection.InsertOneAsync(visualizerDetailDbo, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new VisualizerCreatedNotification { VisualizerId = notification.VisualizerId }, cancellationToken);
        }

        public async Task Handle(VisualizerNameChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = Builders<VisualizerViewDbo>.Update.Set(g => g.Name, notification.Name);
            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(VisualizerExternalIdChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = Builders<VisualizerViewDbo>.Update.Set(g => g.ExternalId, notification.ExternalId);
            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(VisualizerStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = Builders<VisualizerViewDbo>.Update.Set(g => g.StatusId, notification.Status.Id);
            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(VisualizerDeletedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerViewDbo>.Update.Set(g => g.IsDeleted, true);
            await collectionProvider.VisualizerDetailCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new VisualizerDeletedNotification { VisualizerId = notification.VisualizerId }, cancellationToken);
        }

        private async Task UpdateInternalAsync(VisualizerEvent notification, CancellationToken cancellationToken, UpdateDefinition<VisualizerViewDbo> updateDefinition)
        {
            await collectionProvider.VisualizerDetailCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateDefinition, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new VisualizerUpdatedNotification { VisualizerId = notification.VisualizerId }, cancellationToken);
        }
    }
}
