using Game.Contract;

namespace PlayerClient.Contract
{
    public interface IPlayerClient
    {
        Task Ready();
        Task Unready();
        Task Leave();
        Task RequestGamePiecePlacementAcknowledgement(BoardPosition notificationPosition);
    }
}