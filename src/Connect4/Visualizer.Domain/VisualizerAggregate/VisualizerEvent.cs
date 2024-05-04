using Shared.Domain;

namespace Visualizer.Domain.VisualizerAggregate
{
    public record  VisualizerEvent : DomainEvent
    {
        public required VisualizerId VisualizerId { get; init; }
    }
}