namespace Visualizer.Physical.Infrastructure
{
    internal class VisualizerStatusCodeNotValidException : Exception
    {
        public VisualizerStatusCodeNotValidException() : base()
        {
        }

        public VisualizerStatusCodeNotValidException(string? message) : base(message)
        {
        }

        public VisualizerStatusCodeNotValidException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}