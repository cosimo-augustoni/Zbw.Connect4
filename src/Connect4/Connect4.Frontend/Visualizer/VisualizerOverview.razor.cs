using Connect4.Frontend.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using Visualizer.Contract.Queries;

namespace Connect4.Frontend.Visualizer
{
    public partial class VisualizerOverview : IDisposable
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        private VisualizerChangedEventHandler VisualizerChangedEventHandler { get; set; } = null!;

        private bool IsLoading { get; set; }

        public IReadOnlyList<VisualizerDto> Visualizers { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            await this.LoadVisualizers();
            this.VisualizerChangedEventHandler.VisualizerCreated += this.OnVisualizerCreated;
            this.VisualizerChangedEventHandler.VisualizerDeleted += this.OnVisualizerDeleted;
            await base.OnInitializedAsync();
        }

        private async Task OnVisualizerDeleted(object sender, VisualizerChangedEventArgs e)
        {
            await this.LoadVisualizers();
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task OnVisualizerCreated(object sender, VisualizerChangedEventArgs e)
        {
            await this.LoadVisualizers();
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task LoadVisualizers()
        {
            try
            {
                this.IsLoading = true;
                this.Visualizers = await this.Mediator.Send(new AllVisualizersQuery());
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        public void Dispose()
        {
            this.VisualizerChangedEventHandler.VisualizerCreated -= this.OnVisualizerCreated;
            this.VisualizerChangedEventHandler.VisualizerDeleted -= this.OnVisualizerDeleted;
        }
    }
}
