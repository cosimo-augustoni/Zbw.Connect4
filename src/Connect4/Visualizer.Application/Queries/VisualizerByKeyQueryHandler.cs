using Shared.Application;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    internal class VisualizerByKeyQueryHandler(IVisualizerDetailQuery query) : IQueryHandler<VisualizerByKeyQuery, VisualizerDetail>
    {
        public async Task<VisualizerDetail> Handle(VisualizerByKeyQuery request, CancellationToken cancellationToken)
        {
            return await query.GetByIdAsync(request.VisualizerId, cancellationToken);
        }
    }
}