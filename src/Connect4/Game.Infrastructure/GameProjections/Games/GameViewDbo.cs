using Game.Contract;
using MongoDB.Bson;

namespace Game.Infrastructure.GameProjections.Games
{
    internal class GameViewDbo
    {
        public ObjectId Id { get; set; }
        public required Guid GameId { get; set; }
        public required string Name { get; set; }
        
        public required PlayerViewDbo? RedPlayer { get; set; }
        public required PlayerViewDbo? YellowPlayer { get; set; }

        public required Board Board { get; set; }
        public required Guid? CurrentPlayerId { get; set; }
        public required Guid? WinningPlayerId { get; set; }

        public required bool IsFinished { get; set; }
        public required bool IsAborted { get; set; }
        public required bool IsRunning { get; set; }
    }
}
