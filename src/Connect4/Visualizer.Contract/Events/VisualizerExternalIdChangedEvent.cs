namespace Visualizer.Contract.Events
{
    public record VisualizerExternalIdChangedEvent : VisualizerEvent
    {
        public required string? PreviousExternalId { get; init; }
        public required string ExternalId { get; init; }
    }
}