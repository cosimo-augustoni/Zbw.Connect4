using MediatR;
using Visualizer.Contract;
using Visualizer.Contract.Queries;

namespace Connect4.Frontend.Shared
{
    internal class VisualizerChangedEventHandler
    {
        public event AsyncEventHandler<VisualizerChangedEventArgs>? VisualizerUpdated;
        public event AsyncEventHandler<VisualizerChangedEventArgs>? VisualizerCreated;
        public event AsyncEventHandler<VisualizerChangedEventArgs>? VisualizerDeleted;

        internal async Task NotifyUpdatedAsync(VisualizerId id)
        {
            if (this.VisualizerUpdated != null)
                await this.VisualizerUpdated.Invoke(this, new VisualizerChangedEventArgs { VisualizerId = id });
        }

        internal async Task NotifyCreationAsync(VisualizerId id)
        {
            if (this.VisualizerCreated != null)
                await this.VisualizerCreated.Invoke(this, new VisualizerChangedEventArgs { VisualizerId = id });
        }

        internal async Task NotifyDeletionAsync(VisualizerId id)
        {
            if (this.VisualizerDeleted != null)
                await this.VisualizerDeleted.Invoke(this, new VisualizerChangedEventArgs { VisualizerId = id });
        }
    }

    internal class VisualizerChangedNotificationHandler(VisualizerChangedEventHandler visualizerChangedEventHandler) 
        : INotificationHandler<VisualizerUpdatedNotification>,
            INotificationHandler<VisualizerCreatedNotification>,
            INotificationHandler<VisualizerDeletedNotification>
    {
        public async Task Handle(VisualizerUpdatedNotification notification, CancellationToken cancellationToken)
        {
            await visualizerChangedEventHandler.NotifyUpdatedAsync(notification.VisualizerId);
        }

        public async Task Handle(VisualizerCreatedNotification notification, CancellationToken cancellationToken)
        {
            await visualizerChangedEventHandler.NotifyCreationAsync(notification.VisualizerId);
        }

        public async Task Handle(VisualizerDeletedNotification notification, CancellationToken cancellationToken)
        {
            await visualizerChangedEventHandler.NotifyDeletionAsync(notification.VisualizerId);
        }
    }

    internal class VisualizerChangedEventArgs : EventArgs
    {
        internal required VisualizerId VisualizerId { get; init; }
    }
}
