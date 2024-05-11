using Microsoft.AspNetCore.Components;
using MudBlazor;
using Visualizer.Contract;

namespace Connect4.Frontend.Visualizer
{
    public partial class VisualizerStatusDisplay
    {
        [Parameter]
        public required VisualizerStatus VisualizerStatus { get; init; }

        private Color StatusColor
        {
            get => this.VisualizerStatus.StatusType switch
                {
                    StatusType.Ready => Color.Success,
                    StatusType.Busy => Color.Warning,
                    StatusType.Faulty => Color.Error,
                    _ => throw new ArgumentOutOfRangeException()
                };
        }
    }
}