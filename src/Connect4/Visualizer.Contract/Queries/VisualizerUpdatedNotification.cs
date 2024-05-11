using MediatR;

namespace Visualizer.Contract.Queries
{
    public record VisualizerUpdatedNotification : VisualizerNotification;
    public record VisualizerCreatedNotification : VisualizerNotification;
    public record VisualizerDeletedNotification : VisualizerNotification;

    public abstract record VisualizerNotification : INotification
    {
        public required VisualizerId VisualizerId { get; init; }
    }
}
