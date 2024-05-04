using MongoDB.Driver;
using Visualizer.Contract;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Infrastructure.VisualizerProjections.Summary
{
    internal class VisualizersQuery(IVisualizerCollectionProvider collectionProvider) : IVisualizersQuery
    {
        public Task<IReadOnlyList<VisualizerSummary>> GetAllVisualizers(CancellationToken cancellationToken = default)
        {
            var visualizerDetails = collectionProvider.VisualizerSummaryCollection
                .AsQueryable()
                .Where(v => !v.IsDeleted)
                .ToList();

            return Task.FromResult<IReadOnlyList<VisualizerSummary>>(visualizerDetails.Select(v => new VisualizerSummary
            {
                Id = new VisualizerId(v.VisualizerId),
                Name = v.Name,
                ExternalId = v.ExternalId
            }).ToList());
        }
    }
}
