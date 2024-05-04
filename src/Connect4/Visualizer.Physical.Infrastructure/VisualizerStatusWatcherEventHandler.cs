using MediatR;
using Visualizer.Contract.Events;

namespace Visualizer.Physical.Infrastructure
{
    internal class VisualizerStatusWatcherEventHandler(IVisualizerStatusWatcher visualizerStatusWatcher) : INotificationHandler<VisualizerCreatedEvent>, INotificationHandler<VisualizerDeletedEvent>
    {
        public Task Handle(VisualizerCreatedEvent notification, CancellationToken cancellationToken)
        {
            visualizerStatusWatcher.AddVisualizer(notification.VisualizerId);
            return Task.CompletedTask;
        }

        public Task Handle(VisualizerDeletedEvent notification, CancellationToken cancellationToken)
        {
            visualizerStatusWatcher.RemoveVisualizer(notification.VisualizerId);
            return Task.CompletedTask;
        }
    }
}