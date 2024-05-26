using Game.Contract;
using Game.Contract.Queries.Dtos;
using PlayerClient.Contract;

namespace Connect4.Frontend.Game.Games
{
    public record PlayerUIClient
    {
        public required IPlayerClient? PlayerClient { get; init; }

        public required PlayerDto? Player { get; init; }

        public required PlayerSide PlayerSide { get; init; }
    }
}