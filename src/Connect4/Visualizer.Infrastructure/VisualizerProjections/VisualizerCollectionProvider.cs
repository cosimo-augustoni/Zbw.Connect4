using MongoDB.Driver;

namespace Visualizer.Infrastructure.VisualizerProjections
{
    internal class VisualizerCollectionProvider(IMongoDatabase database) : IVisualizerCollectionProvider
    {
        public IMongoCollection<VisualizerViewDbo> VisualizerDetailCollection =>
            database.GetCollection<VisualizerViewDbo>("visualizer_details");
    }
}