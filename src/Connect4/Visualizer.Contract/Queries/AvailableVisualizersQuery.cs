using Shared.Contract;

namespace Visualizer.Contract.Queries
{
    public class AvailableVisualizersQuery : IQuery<IReadOnlyList<VisualizerDto>>;
}