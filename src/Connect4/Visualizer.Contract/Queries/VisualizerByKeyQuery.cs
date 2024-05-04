using Shared.Contract;

namespace Visualizer.Contract.Queries
{
    public class VisualizerByKeyQuery : IQuery<VisualizerDetailDto>
    {
        public required VisualizerId VisualizerId { get; init; }
    }
}