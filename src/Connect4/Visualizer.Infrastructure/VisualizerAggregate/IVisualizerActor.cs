using Orleans;
using Visualizer.Domain.VisualizerAggregate;

namespace Visualizer.Infrastructure.VisualizerAggregate
{
    public interface IVisualizerActor : IVisualizer, IGrainWithGuidKey;
}