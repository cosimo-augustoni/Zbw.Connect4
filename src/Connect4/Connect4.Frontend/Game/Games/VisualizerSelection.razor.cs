using MediatR;
using Microsoft.AspNetCore.Components;
using Connect4.Frontend.Shared;
using Game.Contract;
using Visualizer.Contract;
using Visualizer.Contract.Commands;
using Visualizer.Contract.Queries;

namespace Connect4.Frontend.Game.Games
{
    public partial class VisualizerSelection : IDisposable
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        private VisualizerChangedEventHandler VisualizerChangedEventHandler { get; set; } = null!;

        [Parameter]
        public required GameId GameId { get; set; }

        private IReadOnlyList<VisualizerDto> AvailableVisualizers { get; set; } = [];

        private VisualizerDto? SelectedVisualizer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.VisualizerChangedEventHandler.VisualizerCreated += this.OnVisualizerChanged;
            this.VisualizerChangedEventHandler.VisualizerDeleted += this.OnVisualizerChanged;
            this.VisualizerChangedEventHandler.VisualizerUpdated += this.OnVisualizerChanged;
            await this.LoadVisualizersAsync();

            await base.OnInitializedAsync();
        }

        private async Task LoadVisualizersAsync()
        {
            this.AvailableVisualizers = await this.Mediator.Send(new AvailableVisualizersQuery());
            this.SelectedVisualizer = await this.Mediator.Send(new VisualizerByGameIdQuery { GameId = this.GameId });
        }

        private async Task OnVisualizerChanged(object sender, VisualizerChangedEventArgs e)
        {
            await this.LoadVisualizersAsync();
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task ChangeVisualizer(VisualizerId? visualizerId)
        {
            if (this.SelectedVisualizer != null)
                await this.Mediator.Send(new RemoveVisualizerFromGameCommand() { VisualizerId = this.SelectedVisualizer.Id });

            if (visualizerId == null)
                return;

            await this.Mediator.Send(new AddVisualizerToGameCommand { GameId = this.GameId, VisualizerId = visualizerId });
        }

        public void Dispose()
        {
            this.VisualizerChangedEventHandler.VisualizerCreated -= this.OnVisualizerChanged;
            this.VisualizerChangedEventHandler.VisualizerDeleted -= this.OnVisualizerChanged;
            this.VisualizerChangedEventHandler.VisualizerUpdated -= this.OnVisualizerChanged;
        }
    }
}
