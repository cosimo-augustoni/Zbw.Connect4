namespace PlayerClient.Contract
{
    public interface IPlayerClient
    {
        Task Ready();
        Task Unready();
        Task Leave();
    }
}