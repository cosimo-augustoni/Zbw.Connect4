using Shared.Application;
using Visualizer.Contract.Queries;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    internal class VisualizerByGameIdQueryHandler(IVisualizerQuery query) : IQueryHandler<VisualizerByGameIdQuery, VisualizerDto?>
    {
        public async Task<VisualizerDto?> Handle(VisualizerByGameIdQuery request, CancellationToken cancellationToken)
        {
            var visualizer = await query.GetByGameIdAsync(request.GameId, cancellationToken);
            if (visualizer == null)
                return null;

            return new VisualizerDto
            {
                Id = visualizer.Id,
                Name = visualizer.Name,
                ExternalId = visualizer.ExternalId,
                Status = visualizer.Status
            };
        }
    }
}