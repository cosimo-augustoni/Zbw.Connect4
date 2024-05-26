using Game.Contract;

namespace PlayerClient.Contract
{
    public interface IPlayerClientConnector
    {
        string DisplayName { get; }

        Task JoinGame(GameId gameId, User user, PlayerSide playerSide);
    }
}