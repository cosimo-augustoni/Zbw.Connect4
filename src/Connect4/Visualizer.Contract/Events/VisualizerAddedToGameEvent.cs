using Game.Contract;

namespace Visualizer.Contract.Events
{
    public record VisualizerAddedToGameEvent : VisualizerEvent
    {
        public required GameId GameId { get; init; }
    }
}