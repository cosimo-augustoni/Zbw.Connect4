using MongoDB.Bson;

namespace Visualizer.Infrastructure.VisualizerProjections.Summary
{
    internal class VisualizerSummaryDbo
    {
        public ObjectId Id { get; set; }
        public required Guid VisualizerId { get; set; }
        public string? Name { get; set; }
    }
}