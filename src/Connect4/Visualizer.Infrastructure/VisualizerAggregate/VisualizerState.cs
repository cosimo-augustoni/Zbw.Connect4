namespace Visualizer.Infrastructure.VisualizerAggregate
{
    [Serializable]
    public class VisualizerState
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? ExternalId { get; set; }

        public Guid? CurrentGameId { get; set; }

        public int? StatusId { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
    }
}