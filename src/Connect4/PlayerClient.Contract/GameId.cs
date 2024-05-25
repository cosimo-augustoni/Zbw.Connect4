namespace PlayerClient.Contract
{
    public record GameId(Guid Id)
    {
        public GameId() : this(Guid.NewGuid())
        {
        }
    }
}