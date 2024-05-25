using Game.Contract;
using Game.Contract.Commands;
using Game.Contract.Events;
using Game.Contract.Queries;
using Game.Contract.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace Connect4.Frontend.Game
{
    public partial class GamePage : IDisposable
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private GameChangedEventHandler GameChangedEventHandler { get; set; } = null!;

        [Parameter]
        public Guid GameId { get; set; }

        private GameDto? Game { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Game = await this.Mediator.Send(new GameByIdQuery { Id = new GameId(this.GameId) });
            this.GameChangedEventHandler.GameChanged += this.OnGameChanged;
            await base.OnInitializedAsync();
        }

        private async Task OnGameChanged(object sender, GameChangedEventArgs e)
        {
            this.Game = await this.Mediator.Send(new GameByIdQuery { Id = new GameId(this.GameId) });
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task AbortGame()
        {
            await this.Mediator.Send(new AbortGameCommand(new GameId(this.GameId)));
            this.NavigationManager.NavigateTo("");
        }

        private void CloseGame()
        {
            this.NavigationManager.NavigateTo("");
        }

        public void Dispose()
        {
            this.GameChangedEventHandler.GameChanged -= this.OnGameChanged;
        }
    }
}
