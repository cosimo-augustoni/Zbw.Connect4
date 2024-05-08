using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Visualizer.Contract;
using Visualizer.Contract.Events;
using Visualizer.Contract.Queries;

namespace Connect4.Frontend.Components.Pages
{

    public delegate Task AsyncEventHandler<in T>(object sender, T e) where T : EventArgs;

    internal class VisualizerUpdateEventHandler
    {
        public event AsyncEventHandler<VisualizerUpdatedEventArgs>? VisualizerUpdated;

        internal async Task UpdateAsync(VisualizerId id)
        {
            if (this.VisualizerUpdated != null)
                await this.VisualizerUpdated.Invoke(this, new VisualizerUpdatedEventArgs { VisualizerId = id });
        }
    }

    internal class VisualizerUpdateNotificationHandler(VisualizerUpdateEventHandler visualizerUpdateEventHandler) : INotificationHandler<VisualizerSummaryUpdatedNotification>
    {
        public async Task Handle(VisualizerSummaryUpdatedNotification notification, CancellationToken cancellationToken)
        {
            await visualizerUpdateEventHandler.UpdateAsync(notification.VisualizerId);
        }
    }

    internal class VisualizerUpdatedEventArgs : EventArgs
    {
        internal required VisualizerId VisualizerId { get; init; }
    }
}
