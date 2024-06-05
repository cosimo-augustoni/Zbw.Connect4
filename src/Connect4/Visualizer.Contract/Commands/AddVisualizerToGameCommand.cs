using Game.Contract;
using Shared.Contract;

namespace Visualizer.Contract.Commands
{
    public class AddVisualizerToGameCommand : ICommand
    {
        public required VisualizerId VisualizerId { get; init; }
        public required GameId GameId { get; init; }
    }
}