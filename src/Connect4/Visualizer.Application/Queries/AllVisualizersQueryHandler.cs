using Shared.Application;
using Visualizer.Contract.Queries;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    internal class AllVisualizersQueryHandler(IVisualizerQuery query) : IQueryHandler<AllVisualizersQuery, IReadOnlyList<VisualizerDto>>
    {
        public async Task<IReadOnlyList<VisualizerDto>> Handle(AllVisualizersQuery request, CancellationToken cancellationToken)
        {
            var visualizers = await query.GetAllVisualizersAsync(cancellationToken);
            return visualizers.Select(v => new VisualizerDto
            {
                Id = v.Id,
                Name = v.Name,
                ExternalId = v.ExternalId,
                Status = v.Status,
                IsInGame = v.IsInGame,
            }).ToList();
        }
    }
}