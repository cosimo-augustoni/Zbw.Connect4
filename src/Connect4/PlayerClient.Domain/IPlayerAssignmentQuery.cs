using PlayerClient.Contract;

namespace PlayerClient.Domain
{
    public interface IPlayerAssignmentQuery
    {
        Task<GameId> GetGameIdByPlayerAsync(PlayerId playerId);
    }
}
