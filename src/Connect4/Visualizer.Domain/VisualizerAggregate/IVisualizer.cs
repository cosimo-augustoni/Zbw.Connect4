using Visualizer.Contract;

namespace Visualizer.Domain.VisualizerAggregate
{
    public interface IVisualizer
    {
        Task<Guid> CreateVisualizer(string name, string externalId);
        Task ChangeNameAsync(string name);
        Task ChangeExternalIdAsync(string externalId);
        Task ChangeStatusAsync(VisualizerStatus status);
        Task DeleteAsync();
    }
}