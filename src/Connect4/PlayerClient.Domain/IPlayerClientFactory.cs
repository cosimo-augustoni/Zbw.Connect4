using Game.Contract;
using PlayerClient.Contract;

namespace PlayerClient.Domain
{
    public interface IPlayerClientFactory
    {
        List<PlayerClientType> PlayerClientTypes { get; }
        Task<IPlayerClient?> CreateAsync(PlayerId playerId);
    }
}