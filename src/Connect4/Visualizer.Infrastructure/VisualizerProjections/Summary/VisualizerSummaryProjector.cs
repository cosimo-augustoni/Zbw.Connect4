using MediatR;
using MongoDB.Driver;
using Visualizer.Contract.Events;
using Visualizer.Contract.Queries;
using Visualizer.Infrastructure.VisualizerProjections.Detail;

namespace Visualizer.Infrastructure.VisualizerProjections.Summary
{
    internal class VisualizerSummaryProjector(IVisualizerCollectionProvider collectionProvider, IPublisher mediator) 
        : INotificationHandler<VisualizerCreatedEvent>
            , INotificationHandler<VisualizerNameChangedEvent>
            , INotificationHandler<VisualizerExternalIdChangedEvent>
            , INotificationHandler<VisualizerStatusChangedEvent>
            , INotificationHandler<VisualizerDeletedEvent>
    {
        public async Task Handle(VisualizerCreatedEvent notification, CancellationToken cancellationToken)
        {
            var visualizerDetailDbo = new VisualizerSummaryDbo
            {
                VisualizerId = notification.VisualizerId.Id,
            };
            await collectionProvider.VisualizerSummaryCollection.InsertOneAsync(visualizerDetailDbo, cancellationToken: cancellationToken);

            await mediator.Publish(new VisualizerSummaryUpdatedNotification
            {
                VisualizerId = notification.VisualizerId
            }, cancellationToken);
        }

        public async Task Handle(VisualizerNameChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerSummaryDbo>.Update.Set(g => g.Name, notification.Name);
            await collectionProvider.VisualizerSummaryCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);

            await mediator.Publish(new VisualizerSummaryUpdatedNotification
            {
                VisualizerId = notification.VisualizerId
            }, cancellationToken);
        }

        public async Task Handle(VisualizerExternalIdChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerSummaryDbo>.Update.Set(g => g.ExternalId, notification.ExternalId);
            await collectionProvider.VisualizerSummaryCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);

            await mediator.Publish(new VisualizerSummaryUpdatedNotification
            {
                VisualizerId = notification.VisualizerId
            }, cancellationToken);
        }

        public async Task Handle(VisualizerStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerSummaryDbo>.Update.Set(g => g.StatusId, notification.Status.Id);
            await collectionProvider.VisualizerSummaryCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);

            await mediator.Publish(new VisualizerSummaryUpdatedNotification
            {
                VisualizerId = notification.VisualizerId
            }, cancellationToken);
        }

        public async Task Handle(VisualizerDeletedEvent notification, CancellationToken cancellationToken)
        {
            var updateNameDefinition = Builders<VisualizerSummaryDbo>.Update.Set(g => g.IsDeleted, true);
            await collectionProvider.VisualizerSummaryCollection
                .FindOneAndUpdateAsync(g => g.VisualizerId == notification.VisualizerId.Id, updateNameDefinition, cancellationToken: cancellationToken);

            await mediator.Publish(new VisualizerSummaryUpdatedNotification
            {
                VisualizerId = notification.VisualizerId
            }, cancellationToken);
        }
    }
}
