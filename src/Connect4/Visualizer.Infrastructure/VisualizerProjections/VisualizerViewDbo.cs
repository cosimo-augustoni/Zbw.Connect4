using MongoDB.Bson;

namespace Visualizer.Infrastructure.VisualizerProjections
{
    internal class VisualizerViewDbo
    {
        public ObjectId Id { get; set; }
        public required Guid VisualizerId { get; set; }
        public string? Name { get; set; }
        public string? ExternalId { get; set; }
        public int StatusId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CurrentGameId { get; set; }
    }
}
