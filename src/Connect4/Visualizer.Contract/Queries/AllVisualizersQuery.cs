using Shared.Contract;

namespace Visualizer.Contract.Queries
{
    public class AllVisualizersQuery : IQuery<IReadOnlyList<VisualizerSummaryDto>>;
}