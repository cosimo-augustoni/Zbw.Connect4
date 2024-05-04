using Shared.Application;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    internal class AllVisualizersQueryHandler(IVisualizersQuery query) : IQueryHandler<AllVisualizersQuery, IReadOnlyList<VisualizerSummary>>
    {
        public async Task<IReadOnlyList<VisualizerSummary>> Handle(AllVisualizersQuery request, CancellationToken cancellationToken)
        {
            return await query.GetAllVisualizers(cancellationToken);
        }
    }
}