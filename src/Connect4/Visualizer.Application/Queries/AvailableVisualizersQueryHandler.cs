using Shared.Application;
using Visualizer.Contract.Queries;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    internal class AvailableVisualizersQueryHandler(IVisualizerQuery query) : IQueryHandler<AvailableVisualizersQuery, IReadOnlyList<VisualizerDto>>
    {
        public async Task<IReadOnlyList<VisualizerDto>> Handle(AvailableVisualizersQuery request, CancellationToken cancellationToken)
        {
            var visualizers = await query.GetAvailableVisualizersAsync(cancellationToken);
            return visualizers.Select(v => new VisualizerDto
            {
                Id = v.Id,
                Name = v.Name,
                ExternalId = v.ExternalId,
                Status = v.Status
            }).ToList();
        }
    }
}