using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;
using PlayerClient.Contract;

namespace Connect4.Frontend.Game.Games
{
    public partial class SurrenderButton
    {
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        [Parameter]
        public required List<PlayerUIClient> AllPlayers { get; set; }

        private IPlayerClient? MyClient { get; set; }

        [MemberNotNullWhen(true, nameof(MyClient))]
        private bool CanSurrender => this.MyClient != null;

        protected override async Task OnInitializedAsync()
        {
            var authState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
            this.MyClient = this.AllPlayers.Where(p => p.Player?.Owner.Identifier == authState.User.GetNameIdentifierId()).Select(p => p.PlayerClient).FirstOrDefault();
            await base.OnInitializedAsync();
        }

        private async Task Surrender()
        {
            if (!this.CanSurrender)
                return;

            await this.MyClient.Surrender();
        }
    }
}
