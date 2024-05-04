namespace Visualizer.Domain.VisualizerProjections
{
    public interface IVisualizersQuery
    {
        Task<IReadOnlyList<VisualizerSummary>> GetAllVisualizers(CancellationToken cancellationToken = default);
    }
}