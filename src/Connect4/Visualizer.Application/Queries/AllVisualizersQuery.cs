using Shared.Application;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    public class AllVisualizersQuery : IQuery<IReadOnlyList<VisualizerSummary>>;
}