using MongoDB.Driver;
using Visualizer.Domain;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Infrastructure.VisualizerProjections.Summary
{
    internal class VisualizersQuery(IVisualizerCollectionProvider collectionProvider) : IVisualizersQuery
    {
        public async Task<IReadOnlyList<VisualizerSummary>> GetAllVisualizers(CancellationToken cancellationToken = default)
        {
            var visualizerDetails = await collectionProvider.VisualizerSummaryCollection.AsQueryable().ToListAsync(cancellationToken);
            return visualizerDetails.Select(v => new VisualizerSummary
            {
                Id = new VisualizerId(v.VisualizerId),
                Name = v.Name,
            }).ToList();
        }
    }
}
