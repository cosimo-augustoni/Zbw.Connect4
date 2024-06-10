using MediatR;
using Microsoft.AspNetCore.Components;
using Game.Contract.Commands;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;

namespace Connect4.Frontend.Game.Lobbies
{
    public partial class GameCreationButtons
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        private bool IsCreating { get; set; }

        private async Task CreateGame()
        {
            Guid gameId;
            try
            {
                this.IsCreating = true;
                var authenticationState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
                gameId = await this.Mediator.Send(new CreateGameCommand { Name = authenticationState.User.GetDisplayName() });
            }
            finally
            {
                this.IsCreating = false;
            }
            this.NavigationManager.NavigateTo($"game/{gameId}");
        }
    }
}
