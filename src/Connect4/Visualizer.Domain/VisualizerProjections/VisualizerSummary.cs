using Visualizer.Contract;

namespace Visualizer.Domain.VisualizerProjections
{
    public class VisualizerSummary
    {
        public required VisualizerId Id { get; init; }
        public string? Name { get; init; }
        public string? ExternalId { get; init; }
        public required VisualizerStatus Status { get; set; }
    }
}