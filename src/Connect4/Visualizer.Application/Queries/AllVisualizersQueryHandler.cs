using Shared.Application;
using Visualizer.Contract.Queries;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    internal class AllVisualizersQueryHandler(IVisualizersQuery query) : IQueryHandler<AllVisualizersQuery, IReadOnlyList<VisualizerSummaryDto>>
    {
        public async Task<IReadOnlyList<VisualizerSummaryDto>> Handle(AllVisualizersQuery request, CancellationToken cancellationToken)
        {
            var visualizers = await query.GetAllVisualizers(cancellationToken);
            return visualizers.Select(v => new VisualizerSummaryDto
            {
                Id = v.Id,
                Name = v.Name,
                ExternalId = v.ExternalId,
                Status = v.Status
            }).ToList();
        }
    }
}