using Game.Contract;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;
using PlayerClient.Contract;
using PlayerClient.Contract.Queries;

namespace Connect4.Frontend.Game
{
    public partial class PlayerSlot
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        [Parameter]
        public required GameId GameId { get; set; }

        [Parameter]
        public required PlayerUIClient PlayerClient { get; set; }

        private bool IsSlotFree => this.PlayerClient.PlayerClient == null || this.PlayerClient.Player == null;

        private IReadOnlyList<IPlayerClientConnector> AvailablePlayerClients { get; set; } = [];

        private User? CurrentUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.AvailablePlayerClients = await this.Mediator.Send(new AvailablePlayerClientsQuery());
            var authenticationState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authenticationState.User.GetNameIdentifierId();
            var userDisplayName = authenticationState.User.GetDisplayName();
            this.CurrentUser = new User(userId!, userDisplayName!);
        }

        private async Task JoinClient(IPlayerClientConnector client)
        {
            if (this.CurrentUser == null)
                return;

            await client.JoinGame(this.GameId, this.CurrentUser, this.PlayerClient.PlayerSide);
        }

        private async Task Leave()
        {
            if (this.PlayerClient.PlayerClient == null)
                return;

            await this.PlayerClient.PlayerClient.Leave();
        }

        private async Task ReadyChanged(bool isReady)
        {
            if (this.PlayerClient.PlayerClient == null)
                return;

            if(isReady)
                await this.PlayerClient.PlayerClient.Ready();
            else
                await this.PlayerClient.PlayerClient.Unready();
        }
    }
}
