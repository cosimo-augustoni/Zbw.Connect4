using Shared.Contract;

namespace Visualizer.Contract.Commands
{
    public class DeleteVisualizerCommand : ICommand
    {
        public required VisualizerId VisualizerId { get; init; }
    }
}