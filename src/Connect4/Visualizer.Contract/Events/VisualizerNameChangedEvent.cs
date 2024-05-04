namespace Visualizer.Contract.Events
{
    public record VisualizerNameChangedEvent : VisualizerEvent
    {
        public required string Name { get; init; }
    }
}