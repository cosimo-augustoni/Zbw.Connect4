using Game.Contract;
using Shared.Contract;

namespace Visualizer.Contract.Queries
{
    public class VisualizerByGameIdQuery : IQuery<VisualizerDto?>
    {
        public required GameId GameId { get; init; }
    }
}