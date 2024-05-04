namespace Visualizer.Domain.VisualizerAggregate
{
    public record VisualizerExternalIdChangedEvent : VisualizerEvent
    {
        public required string ExternalId { get; init; }
    }
}