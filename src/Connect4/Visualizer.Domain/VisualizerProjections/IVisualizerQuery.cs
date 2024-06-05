using Game.Contract;
using Visualizer.Contract;

namespace Visualizer.Domain.VisualizerProjections
{
    public interface IVisualizerQuery
    {
        Task<VisualizerView> GetByIdAsync(VisualizerId id, CancellationToken cancellationToken = default);
        Task<VisualizerView> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<VisualizerView>> GetAllVisualizersAsync(CancellationToken cancellationToken = default);
        Task<VisualizerView?> GetByGameIdAsync(GameId gameId, CancellationToken cancellationToken = default);
    }
}
