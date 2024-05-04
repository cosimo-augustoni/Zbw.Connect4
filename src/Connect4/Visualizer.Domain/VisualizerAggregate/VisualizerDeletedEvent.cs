namespace Visualizer.Domain.VisualizerAggregate
{
    public record VisualizerDeletedEvent : VisualizerEvent
    {
        public required DateTimeOffset DeletedAt { get; init; }
    }
}