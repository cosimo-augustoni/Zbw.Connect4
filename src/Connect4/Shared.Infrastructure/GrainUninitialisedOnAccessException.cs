namespace Shared.Infrastructure
{
    public class GrainUninitialisedOnAccessException : Exception
    {
        public GrainUninitialisedOnAccessException()
        {
        }

        public GrainUninitialisedOnAccessException(string? message) : base(message)
        {
        }

        public GrainUninitialisedOnAccessException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}