using Shared.Application;
using Visualizer.Contract.Queries;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    internal class VisualizerByKeyQueryHandler(IVisualizerQuery query) : IQueryHandler<VisualizerByKeyQuery, VisualizerDto>
    {
        public async Task<VisualizerDto> Handle(VisualizerByKeyQuery request, CancellationToken cancellationToken)
        {
            var visualizer = await query.GetByIdAsync(request.VisualizerId, cancellationToken);
            return new VisualizerDto
            {
                Id = visualizer.Id,
                Name = visualizer.Name,
                ExternalId = visualizer.ExternalId,
                Status = visualizer.Status,
                IsInGame = visualizer.IsInGame,
            };
        }
    }
}