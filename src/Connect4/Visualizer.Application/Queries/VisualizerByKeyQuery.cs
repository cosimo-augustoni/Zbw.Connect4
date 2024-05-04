using Shared.Application;
using Visualizer.Domain;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    public class VisualizerByKeyQuery : IQuery<VisualizerDetail>
    {
        public required VisualizerId VisualizerId { get; init; }
    }
}