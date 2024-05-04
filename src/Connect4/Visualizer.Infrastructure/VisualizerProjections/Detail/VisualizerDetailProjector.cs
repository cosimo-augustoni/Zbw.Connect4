using MediatR;
using MongoDB.Driver;
using Visualizer.Domain.VisualizerAggregate;

namespace Visualizer.Infrastructure.VisualizerProjections.Detail
{
    internal class VisualizerDetailProjector(IVisualizerCollectionProvider collectionProvider) 
        : INotificationHandler<VisualizerCreatedEvent>
            , INotificationHandler<VisualizerNameChangedEvent>
            , INotificationHandler<VisualizerExternalIdChangedEvent>
            , INotificationHandler<VisualizerStatusChangedEvent>
            , INotificationHandler<VisualizerDeletedEvent>
    {
        public async Task Handle(VisualizerCreatedEvent notification, CancellationToken cancellationToken)
        {
            var visualizerDetailDbo = new VisualizerDetailDbo
            {
                VisualizerId = notification.VisualizerId.Id,
            };
            await collectionProvider.VisualizerDetailCollection.InsertOneAsync(visualizerDetailDbo, cancellationToken: cancellationToken);
        }

        public async Task Handle(VisualizerNameChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerDetailDbo>.Update.Set(g => g.Name, notification.Name);
            await collectionProvider.VisualizerDetailCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);
        }

        public async Task Handle(VisualizerExternalIdChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerDetailDbo>.Update.Set(g => g.ExternalId, notification.ExternalId);
            await collectionProvider.VisualizerDetailCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);
        }
        
        public async Task Handle(VisualizerStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerDetailDbo>.Update.Set(g => g.StatusId, notification.Status.Id);
            await collectionProvider.VisualizerDetailCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);
        }

        public async Task Handle(VisualizerDeletedEvent notification, CancellationToken cancellationToken)
        {
            await collectionProvider.VisualizerDetailCollection
                .DeleteOneAsync(g => g.VisualizerId == notification.VisualizerId.Id, cancellationToken: cancellationToken);
        }
    }
}
