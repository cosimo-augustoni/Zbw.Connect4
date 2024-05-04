using Game.Contract;

namespace Game.Domain.GameProjections
{
    public class GameSummary
    {
        public required GameId Id { get; init; }
        public string? Name { get; init; }
    }
}
