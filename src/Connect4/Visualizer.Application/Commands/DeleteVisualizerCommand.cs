using Shared.Application;
using Visualizer.Domain;

namespace Visualizer.Application.Commands
{
    public class DeleteVisualizerCommand : ICommand
    {
        public required VisualizerId VisualizerId { get; init; }
    }
}