using Shared.Contract;

namespace Visualizer.Contract.Queries
{
    public class VisualizerByExternalIdQuery : IQuery<VisualizerDto>
    {
        public required string ExternalId { get; init; }
    }
}