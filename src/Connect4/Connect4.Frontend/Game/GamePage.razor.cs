using Game.Contract;
using Game.Contract.Commands;
using Game.Contract.Events;
using Game.Contract.Queries;
using Game.Contract.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace Connect4.Frontend.Game
{
    public partial class GamePage
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public Guid GameId { get; set; }

        private GameDto? Game { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Game = await this.Mediator.Send(new GameByIdQuery { Id = new GameId(this.GameId) });
            await base.OnInitializedAsync();
        }

        private async Task AbortGame()
        {
            await this.Mediator.Send(new AbortGameCommand(new GameId(this.GameId)));
            this.NavigationManager.NavigateTo("");
        }
    }
}
