namespace PlayerClient.Contract
{
    public record PlayerId(Guid Id)
    {
        public PlayerId() : this(Guid.NewGuid())
        {
        }
    }
}