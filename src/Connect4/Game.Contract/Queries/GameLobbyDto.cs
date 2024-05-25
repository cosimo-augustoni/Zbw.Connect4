namespace Game.Contract.Queries
{
    public class GameLobbyDto
    {
        public required GameId Id { get; init; }
        public required string Name { get; init; }
        public required bool HasOpenPlayerSlot { get; init; }
    }
}