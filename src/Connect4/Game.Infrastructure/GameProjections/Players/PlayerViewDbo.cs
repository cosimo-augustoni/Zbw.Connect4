using Game.Contract;
using MongoDB.Bson;

namespace Game.Infrastructure.GameProjections.Players
{
    internal class PlayerViewDbo
    {
        public ObjectId Id { get; set; }
        public Guid PlayerId { get; set; }
        public required string Name { get; set; }
        public bool IsReady { get; set; }
    }
}
