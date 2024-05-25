using Game.Contract;

namespace PlayerClient.Contract
{
    public interface IPlayerClientConnector
    {
        Task JoinGame(GameId gameId, User user, PlayerSide playerSide);
    }
}