using MongoDB.Bson;

namespace Game.Infrastructure.GameProjections.Games
{
    internal class PlayerViewDbo
    {
        public ObjectId Id { get; set; }
        public Guid PlayerId { get; set; }
        public required PlayerOwnerDbo Owner { get; set; }
        public bool IsReady { get; set; }
        public required string Type { get; set; }
    }
}
