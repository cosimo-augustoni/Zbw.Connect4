using PlayerClient.Contract;

namespace PlayerClient.Domain
{
    public interface IPlayerClientFactory
    {
        PlayerClientType PlayerClientType { get; }
        Task<IPlayerClient> CreateAsync(PlayerId playerId);
    }
}