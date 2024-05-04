using Shared.Contract;

namespace Visualizer.Contract.Commands
{
    public class ChangeVisualizerStatusCommand : ICommand
    {
        public required VisualizerId VisualizerId { get; init; }

        public required VisualizerStatus Status { get; init; }
    }
}