using Shared.Application;
using Visualizer.Domain;
using Visualizer.Domain.VisualizerAggregate;

namespace Visualizer.Application.Commands
{
    public class ChangeVisualizerStatusCommand : ICommand
    {
        public required VisualizerId VisualizerId { get; init; }

        public required VisualizerStatus Status { get; init; }
    }
}