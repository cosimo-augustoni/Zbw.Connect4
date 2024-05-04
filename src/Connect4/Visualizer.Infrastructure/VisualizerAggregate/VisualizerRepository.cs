using Orleans;
using Visualizer.Contract;
using Visualizer.Domain.VisualizerAggregate;

namespace Visualizer.Infrastructure.VisualizerAggregate
{
    internal class VisualizerRepository(IGrainFactory grainFactory) : IVisualizerRepository
    {
        public IVisualizer Create()
        {
            return grainFactory.GetGrain<IVisualizerActor>(new VisualizerId().Id);
        }

        public IVisualizer GetById(VisualizerId id)
        {
            return grainFactory.GetGrain<IVisualizerActor>(id.Id);
        }
    }
}
