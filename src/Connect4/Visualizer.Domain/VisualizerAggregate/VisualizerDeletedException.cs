namespace Visualizer.Domain.VisualizerAggregate
{
    public class VisualizerDeletedException : Exception
    {
        public VisualizerDeletedException() : base()
        {
        }

        public VisualizerDeletedException(string? message) : base(message)
        {
        }

        public VisualizerDeletedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}