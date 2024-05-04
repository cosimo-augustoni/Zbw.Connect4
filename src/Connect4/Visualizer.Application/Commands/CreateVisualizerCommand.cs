using Shared.Application;

namespace Visualizer.Application.Commands
{
    public class CreateVisualizerCommand : ICommand<Guid>
    {
        public required string Name { get; init; }
        public required string ExternalId { get; init; }
    }
}
