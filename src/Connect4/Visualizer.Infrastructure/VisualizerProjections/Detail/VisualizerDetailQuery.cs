using MongoDB.Driver;
using Visualizer.Contract;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Infrastructure.VisualizerProjections.Detail
{
    internal class VisualizerDetailQuery(IVisualizerCollectionProvider collectionProvider) : IVisualizerDetailQuery
    {
        public Task<VisualizerDetail> GetByIdAsync(VisualizerId id, CancellationToken cancellationToken = default)
        {
            var visualizerDetail = collectionProvider.VisualizerDetailCollection
                .AsQueryable()
                .First(v => v.VisualizerId == id.Id);

            return Task.FromResult(new VisualizerDetail
            {
                Id = new VisualizerId(visualizerDetail.VisualizerId),
                Name = visualizerDetail.Name,
                ExternalId = visualizerDetail.ExternalId,
                Status = VisualizerStatus.GetById(visualizerDetail.StatusId)
            });
        }
    }
}
