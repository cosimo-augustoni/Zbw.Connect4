using MediatR;
using Visualizer.Contract.Events;
using Visualizer.Contract.Queries;

namespace Visualizer.Physical.Infrastructure
{
    internal class VisualizerSubscriptionManagementEventHandler(IVisualizerMqttClient mqttClient, ISender mediator)
        : INotificationHandler<VisualizerExternalIdChangedEvent>
            , INotificationHandler<VisualizerDeletedEvent>
    {
        public async Task Handle(VisualizerExternalIdChangedEvent notification, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(notification.PreviousExternalId))
                await mqttClient.UnsubscribeWithExternalIdAsync(notification.PreviousExternalId);

            await mqttClient.SubscribeWithExternalIdAsync(notification.ExternalId);
        }

        public async Task Handle(VisualizerDeletedEvent notification, CancellationToken cancellationToken)
        {
            var visualizer = await mediator.Send(new VisualizerByKeyQuery { VisualizerId = notification.VisualizerId }, cancellationToken);
            if (!string.IsNullOrWhiteSpace(visualizer.ExternalId))
                await mqttClient.UnsubscribeWithExternalIdAsync(visualizer.ExternalId);
        }
    }
}