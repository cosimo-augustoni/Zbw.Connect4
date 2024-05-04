using MediatR;
using MongoDB.Driver;
using Visualizer.Contract.Events;

namespace Visualizer.Infrastructure.VisualizerProjections.Summary
{
    internal class VisualizerSummaryProjector(IVisualizerCollectionProvider collectionProvider) 
        : INotificationHandler<VisualizerCreatedEvent>
            , INotificationHandler<VisualizerNameChangedEvent>
            , INotificationHandler<VisualizerExternalIdChangedEvent>
            , INotificationHandler<VisualizerDeletedEvent>
    {
        public async Task Handle(VisualizerCreatedEvent notification, CancellationToken cancellationToken)
        {
            var visualizerDetailDbo = new VisualizerSummaryDbo
            {
                VisualizerId = notification.VisualizerId.Id,
            };
            await collectionProvider.VisualizerSummaryCollection.InsertOneAsync(visualizerDetailDbo, cancellationToken: cancellationToken);
        }

        public async Task Handle(VisualizerNameChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerSummaryDbo>.Update.Set(g => g.Name, notification.Name);
            await collectionProvider.VisualizerSummaryCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);
        }

        public async Task Handle(VisualizerExternalIdChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerSummaryDbo>.Update.Set(g => g.ExternalId, notification.ExternalId);
            await collectionProvider.VisualizerSummaryCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);
        }

        public async Task Handle(VisualizerDeletedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerSummaryDbo>.Update.Set(g => g.IsDeleted, true);
            await collectionProvider.VisualizerSummaryCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);
        }
    }
}
