namespace Visualizer.Contract.Events
{
    public record VisualizerStatusChangedEvent : VisualizerEvent
    {
        public required VisualizerStatus Status { get; init; }
    }
}