namespace Visualizer.Contract.Queries
{
    public class VisualizerSummaryDto
    {
        public required VisualizerId Id { get; init; }
        public string? Name { get; init; }
    }
}