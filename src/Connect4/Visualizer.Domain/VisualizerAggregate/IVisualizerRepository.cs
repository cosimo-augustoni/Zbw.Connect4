namespace Visualizer.Domain.VisualizerAggregate
{
    public interface IVisualizerRepository
    {
        public IVisualizer Create();

        public IVisualizer GetById(VisualizerId id);
    }
}
