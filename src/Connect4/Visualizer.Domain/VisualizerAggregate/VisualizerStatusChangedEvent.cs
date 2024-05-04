namespace Visualizer.Domain.VisualizerAggregate
{
    public record VisualizerStatusChangedEvent : VisualizerEvent
    {
        public required VisualizerStatus Status { get; init; }
    }
}