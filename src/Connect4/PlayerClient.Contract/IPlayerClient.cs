using Game.Contract;

namespace PlayerClient.Contract
{
    public interface IPlayerClient
    {
        Task Ready();
        Task Unready();
        Task Leave();
        Task Surrender();
        Task RequestGamePiecePlacementAcknowledgement(BoardPosition notificationPosition);
    }
}