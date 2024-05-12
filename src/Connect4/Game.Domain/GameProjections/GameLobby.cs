using Game.Contract;

namespace Game.Domain.GameProjections
{
    public class GameLobby
    {
        public required GameId Id { get; init; }
        public required string Name { get; init; }
        public required bool HasOpenPlayerSlot { get; init; }
    }
}
