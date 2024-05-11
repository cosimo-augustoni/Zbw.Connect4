using Shared.Contract;

namespace Visualizer.Contract.Queries
{
    public class VisualizerByKeyQuery : IQuery<VisualizerDto>
    {
        public required VisualizerId VisualizerId { get; init; }
    }
}