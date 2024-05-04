namespace Visualizer.Contract
{
    public record VisualizerId(Guid Id)
    {
        public VisualizerId() : this(Guid.NewGuid())
        {
        }
    }
}