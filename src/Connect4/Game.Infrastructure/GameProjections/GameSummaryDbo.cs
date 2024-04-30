using MongoDB.Bson;

namespace Game.Infrastructure.GameProjections
{
    internal class GameSummaryDbo
    {
        public ObjectId Id { get; set; }
        public required Guid GameId { get; set; }
        public string? Name { get; set; }
    }
}