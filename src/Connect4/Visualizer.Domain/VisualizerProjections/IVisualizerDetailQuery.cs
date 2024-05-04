using Visualizer.Contract;

namespace Visualizer.Domain.VisualizerProjections
{
    public interface IVisualizerDetailQuery
    {
        Task<VisualizerDetail> GetByIdAsync(VisualizerId id, CancellationToken cancellationToken = default);
    }
}
