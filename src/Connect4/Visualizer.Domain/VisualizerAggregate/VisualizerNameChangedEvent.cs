namespace Visualizer.Domain.VisualizerAggregate
{
    public record VisualizerNameChangedEvent : VisualizerEvent
    {
        public required string Name { get; init; }
    }
}