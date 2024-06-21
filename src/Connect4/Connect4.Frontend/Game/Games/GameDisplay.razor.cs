using Connect4.Frontend.Shared;
using Game.Contract.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualBasic;
using Visualizer.Contract.Queries;

namespace Connect4.Frontend.Game.Games
{
    public partial class GameDisplay : IDisposable
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private VisualizerChangedEventHandler VisualizerChangedEventHandler { get; set; } = null!;

        [Parameter]
        public required GameDto Game { get; set; }

        [Parameter]
        public required PlayerUIClient YellowPlayer { get; set; }

        [Parameter]
        public required PlayerUIClient RedPlayer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.VisualizerChangedEventHandler.VisualizerUpdated += this.OnVisualizerChanged;
            await this.LoadVisualizerAsync();
            await base.OnInitializedAsync();
        }

        private async Task OnVisualizerChanged(object sender, VisualizerChangedEventArgs e)
        {
            await this.LoadVisualizerAsync();
            await this.InvokeAsync(this.StateHasChanged);
        }

        public VisualizerDto? Visualizer { get; set; }

        private async Task LoadVisualizerAsync()
        {
            this.Visualizer = await this.Mediator.Send(new VisualizerByGameIdQuery { GameId = this.Game.Id });
        }

        public void Dispose()
        {
            this.VisualizerChangedEventHandler.VisualizerUpdated -= this.OnVisualizerChanged;
        }
    }
}
