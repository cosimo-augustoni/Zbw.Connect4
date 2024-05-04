using MongoDB.Driver;
using Visualizer.Infrastructure.VisualizerProjections.Detail;
using Visualizer.Infrastructure.VisualizerProjections.Summary;

namespace Visualizer.Infrastructure.VisualizerProjections
{
    internal class VisualizerCollectionProvider(IMongoDatabase database) : IVisualizerCollectionProvider
    {
        public IMongoCollection<VisualizerSummaryDbo> VisualizerSummaryCollection =>
            database.GetCollection<VisualizerSummaryDbo>("visualizer_summaries");

        public IMongoCollection<VisualizerDetailDbo> VisualizerDetailCollection =>
            database.GetCollection<VisualizerDetailDbo>("visualizer_details");
    }
}