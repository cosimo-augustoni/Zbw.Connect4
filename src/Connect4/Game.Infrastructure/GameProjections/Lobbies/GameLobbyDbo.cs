using MongoDB.Bson;

namespace Game.Infrastructure.GameProjections.Lobbies
{
    internal class GameLobbyDbo
    {
        public ObjectId Id { get; set; }
        public required Guid GameId { get; set; }
        public required string Name { get; set; }
        public int OpenPlayerSlots { get; set; }
    }
}