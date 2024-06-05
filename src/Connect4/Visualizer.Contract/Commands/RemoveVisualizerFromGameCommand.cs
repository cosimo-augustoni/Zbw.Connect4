using Shared.Contract;

namespace Visualizer.Contract.Commands
{
    public class RemoveVisualizerFromGameCommand : ICommand
    {
        public required VisualizerId VisualizerId { get; init; }
    }
}