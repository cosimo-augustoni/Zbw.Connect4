namespace Visualizer.Contract.Events
{
    public record VisualizerDeletedEvent : VisualizerEvent
    {
        public required DateTimeOffset DeletedAt { get; init; }
    }
}