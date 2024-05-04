using Shared.Contract;

namespace Visualizer.Contract.Commands
{
    public class UpdateVisualizerCommand : ICommand
    {
        public required VisualizerId VisualizerId { get; init; }
        public required string Name { get; init; }
        public required string ExternalId { get; init; }
    }
}
