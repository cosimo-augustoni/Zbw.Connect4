using MongoDB.Bson;

namespace Visualizer.Infrastructure.VisualizerProjections.Detail
{
    internal class VisualizerDetailDbo
    {
        public ObjectId Id { get; set; }
        public required Guid VisualizerId { get; set; }
        public string? Name { get; set; }
        public string? ExternalId { get; set; }
        public int StatusId { get; set; }
    }
}
