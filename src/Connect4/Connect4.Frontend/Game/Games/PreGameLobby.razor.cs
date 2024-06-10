using Game.Contract.Commands;
using Game.Contract.Queries.Dtos;
using Microsoft.AspNetCore.Components;
using MediatR;
using Visualizer.Contract.Queries;
using Connect4.Frontend.Shared;
using Visualizer.Contract;

namespace Connect4.Frontend.Game.Games
{
    public partial class PreGameLobby : IDisposable
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

        private bool CanStartGame => (this.Game.YellowPlayer?.IsReady ?? false) && (this.Game.RedPlayer?.IsReady ?? false) && this.Visualizer?.Status == VisualizerStatus.PiecesSorted;

        public bool IsTitelEditing { get; set; } = false;

        private string GameTitle { get; set; } = string.Empty;

        private VisualizerDto? Visualizer { get; set; }

        protected override Task OnInitializedAsync()
        {
            this.VisualizerChangedEventHandler.VisualizerUpdated += this.OnVisualizerChanged;
            return base.OnInitializedAsync();
        }

        private async Task OnVisualizerChanged(object sender, VisualizerChangedEventArgs e)
        {
            await this.LoadVisualizerAsync();
            await this.InvokeAsync(this.StateHasChanged);
        }

        protected override async Task OnParametersSetAsync()
        {
            this.IsTitelEditing = false;
            this.GameTitle = this.Game.Name;
            await base.OnParametersSetAsync();
        }

        private async Task LoadVisualizerAsync()
        {
            this.Visualizer = await this.Mediator.Send(new VisualizerByGameIdQuery { GameId = this.Game.Id });
        }

        private async Task AbortGame()
        {
            await this.Mediator.Send(new AbortGameCommand(this.Game.Id));
            this.NavigationManager.NavigateTo("");
        }

        private async Task StartGame()
        {
            await this.Mediator.Send(new StartGameCommand(this.Game.Id));
        }

        private Task StartEditTitle()
        {
            this.IsTitelEditing = true;
            return Task.CompletedTask;
        }

        private async Task UpdateGameName()
        {
            this.IsTitelEditing = false;
            await this.Mediator.Send(new UpdateGameNameCommand(this.Game.Id, this.GameTitle));
        }

        public void Dispose()
        {
            this.VisualizerChangedEventHandler.VisualizerUpdated -= this.OnVisualizerChanged;
        }
    }
}
