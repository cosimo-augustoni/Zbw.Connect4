using Game.Contract;
using PlayerClient.Contract;

namespace PlayerClient.Domain
{
    public interface IPlayerAssignmentQuery
    {
        Task<GameId?> GetGameIdByPlayerAsync(PlayerId playerId);

        Task<List<(PlayerId PlayerId, PlayerClientType PlayerType)>> GetPlayersByGameAsync(GameId gameId);
    }
}
