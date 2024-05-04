using Visualizer.Contract;

namespace Visualizer.Physical.Infrastructure
{
    internal interface IVisualizerStatusWatcher
    {
        Task StartAsync();
        void StatusUpdateReceived(VisualizerId visualizerId);
        void AddVisualizer(VisualizerId visualizerId);
        void RemoveVisualizer(VisualizerId visualizerId);
        Task StopAsync();
    }
}