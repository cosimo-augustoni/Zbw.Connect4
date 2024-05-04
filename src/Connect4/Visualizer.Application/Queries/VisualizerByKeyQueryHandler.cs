using Shared.Application;
using Visualizer.Contract.Queries;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    internal class VisualizerByKeyQueryHandler(IVisualizerDetailQuery query) : IQueryHandler<VisualizerByKeyQuery, VisualizerDetailDto>
    {
        public async Task<VisualizerDetailDto> Handle(VisualizerByKeyQuery request, CancellationToken cancellationToken)
        {
            var visualizerDetail = await query.GetByIdAsync(request.VisualizerId, cancellationToken);
            return new VisualizerDetailDto
            {
                Id = visualizerDetail.Id,
                Name = visualizerDetail.Name,
                ExternalId = visualizerDetail.ExternalId,
                Status = visualizerDetail.Status
            };
        }
    }

    internal class VisualizerByExternalIdQueryHandler(IVisualizerDetailQuery query) : IQueryHandler<VisualizerByExternalIdQuery, VisualizerDetailDto>
    {
        public async Task<VisualizerDetailDto> Handle(VisualizerByExternalIdQuery request, CancellationToken cancellationToken)
        {
            var visualizerDetail = await query.GetByExternalIdAsync(request.ExternalId, cancellationToken);
            return new VisualizerDetailDto
            {
                Id = visualizerDetail.Id,
                Name = visualizerDetail.Name,
                ExternalId = visualizerDetail.ExternalId,
                Status = visualizerDetail.Status
            };
        }
    }

    
}