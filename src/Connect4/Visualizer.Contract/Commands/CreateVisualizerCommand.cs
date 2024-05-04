using Shared.Contract;

namespace Visualizer.Contract.Commands
{
    public class CreateVisualizerCommand : ICommand<Guid>
    {
        public required string Name { get; init; }
        public required string ExternalId { get; init; }
    }
}
