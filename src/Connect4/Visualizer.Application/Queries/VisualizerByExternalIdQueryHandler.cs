using Shared.Application;
using Visualizer.Contract.Queries;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    internal class VisualizerByExternalIdQueryHandler(IVisualizerQuery query) : IQueryHandler<VisualizerByExternalIdQuery, VisualizerDto>
    {
        public async Task<VisualizerDto> Handle(VisualizerByExternalIdQuery request, CancellationToken cancellationToken)
        {
            var visualizerDetail = await query.GetByExternalIdAsync(request.ExternalId, cancellationToken);
            return new VisualizerDto
            {
                Id = visualizerDetail.Id,
                Name = visualizerDetail.Name,
                ExternalId = visualizerDetail.ExternalId,
                Status = visualizerDetail.Status
            };
        }
    }
}