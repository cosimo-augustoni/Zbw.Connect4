using MongoDB.Driver;
using Visualizer.Infrastructure.VisualizerProjections.Detail;
using Visualizer.Infrastructure.VisualizerProjections.Summary;

namespace Visualizer.Infrastructure.VisualizerProjections
{
    internal interface IVisualizerCollectionProvider
    {
        IMongoCollection<VisualizerSummaryDbo> VisualizerSummaryCollection { get; }
        IMongoCollection<VisualizerDetailDbo> VisualizerDetailCollection { get; }
    }
}