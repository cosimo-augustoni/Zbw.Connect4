using Game.Contract.Commands;
using Game.Contract.Queries.Dtos;
using Microsoft.AspNetCore.Components;
using MediatR;

namespace Connect4.Frontend.Game.Games
{
    public partial class PreGameLobby
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public required GameDto Game { get; set; }

        [Parameter]
        public required PlayerUIClient YellowPlayer { get; set; }

        [Parameter]
        public required PlayerUIClient RedPlayer { get; set; }

        private bool CanStartGame => (this.Game?.YellowPlayer?.IsReady ?? false) && (this.Game?.RedPlayer?.IsReady ?? false);

        private async Task AbortGame()
        {
            await this.Mediator.Send(new AbortGameCommand(this.Game.Id));
            this.NavigationManager.NavigateTo("");
        }

        private async Task StartGame()
        {
            await this.Mediator.Send(new StartGameCommand(this.Game.Id));
        }
    }
}
