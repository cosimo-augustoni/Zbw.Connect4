using Shared.Contract;

namespace Visualizer.Contract.Queries
{
    public class VisualizerByExternalIdQuery : IQuery<VisualizerDetailDto>
    {
        public required string ExternalId { get; init; }
    }
}