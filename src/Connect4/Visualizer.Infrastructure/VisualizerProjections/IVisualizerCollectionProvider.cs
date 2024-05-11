using MongoDB.Driver;

namespace Visualizer.Infrastructure.VisualizerProjections
{
    internal interface IVisualizerCollectionProvider
    {
        IMongoCollection<VisualizerViewDbo> VisualizerDetailCollection { get; }
    }
}