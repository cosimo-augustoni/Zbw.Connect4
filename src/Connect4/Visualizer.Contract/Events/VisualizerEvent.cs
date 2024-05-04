using Shared.Contract;

namespace Visualizer.Contract.Events
{
    public record  VisualizerEvent : DomainEvent
    {
        public required VisualizerId VisualizerId { get; init; }
    }
}