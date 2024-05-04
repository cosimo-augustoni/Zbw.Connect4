using System.Collections.Concurrent;
using System.Timers;
using MediatR;
using Visualizer.Contract;
using Visualizer.Contract.Commands;
using Visualizer.Contract.Queries;
using Timer = System.Timers.Timer;

namespace Visualizer.Physical.Infrastructure
{
    internal class VisualizerStatusWatcher(ISender mediator) : IVisualizerStatusWatcher
    {
        private readonly ConcurrentDictionary<VisualizerId, TimedStatusUpdate> statusUpdateTimers = new();

        public async Task StartAsync()
        {
            var visualizers = await mediator.Send(new AllVisualizersQuery());
            foreach (var visualizer in visualizers)
            {
                this.statusUpdateTimers.TryAdd(visualizer.Id, new TimedStatusUpdate(mediator, visualizer.Id));
            }
        }

        public void StatusUpdateReceived(VisualizerId visualizerId)
        {
            this.statusUpdateTimers.TryGetValue(visualizerId, out var timer);
            timer?.Reset();
        }

        public void AddVisualizer(VisualizerId visualizerId)
        {
            this.statusUpdateTimers.TryAdd(visualizerId, new TimedStatusUpdate(mediator, visualizerId));
        }

        public void RemoveVisualizer(VisualizerId visualizerId)
        {
            this.statusUpdateTimers.TryRemove(visualizerId, out var timer);
            timer?.Dispose();
        }

        public Task StopAsync()
        {
            foreach (var visualizerId in this.statusUpdateTimers.Keys)
            {
                this.RemoveVisualizer(visualizerId);
            }

            return Task.CompletedTask;
        }

        private class TimedStatusUpdate : IDisposable
        {
            private readonly ISender mediator;
            private readonly VisualizerId visualizerId;
            private readonly Timer timer;
            private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            public TimedStatusUpdate(ISender mediator, VisualizerId visualizerId)
            {
                this.mediator = mediator;
                this.visualizerId = visualizerId;
                this.timer = new Timer(TimeSpan.FromSeconds(8));
                this.timer.AutoReset = true;
                this.timer.Elapsed += this.TimerOnElapsed;
                this.timer.Start();
            }

            private async void TimerOnElapsed(object? sender, ElapsedEventArgs e)
            {
                await this.mediator.Send(new ChangeVisualizerStatusCommand
                {
                    VisualizerId = this.visualizerId,
                    Status = VisualizerStatus.Unknown
                }, this.cancellationTokenSource.Token);
            }

            public void Reset()
            {
                this.timer.Stop();
                this.timer.Start();
            }

            public void Dispose()
            {
                this.cancellationTokenSource.Cancel();
                this.timer.Stop();
                this.timer.Elapsed -= this.TimerOnElapsed;
                this.timer.Dispose();
            }
        }
    }
}