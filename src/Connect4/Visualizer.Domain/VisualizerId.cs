namespace Visualizer.Domain
{
    public record VisualizerId(Guid Id)
    {
        public VisualizerId() : this(Guid.NewGuid())
        {
        }
    }
}