using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;

namespace Connect4.Frontend.Game
{
    public partial class PlayerSlot
    {
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        private AuthenticationState? AuthenticationState { get; set; }

        [MemberNotNullWhen(false, nameof(Player))]
        private bool IsSlotFree => this.Player == null;
        
        private Player? Player { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.AuthenticationState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
        }

        private Task Join()
        {
            if (this.AuthenticationState == null)
                return Task.CompletedTask;

            var userId = this.AuthenticationState.User.GetNameIdentifierId();
            var userDisplayName = this.AuthenticationState.User.GetDisplayName();

            this.Player = new Player
            {
                IsReady = false,
                User = new User
                {
                    Identifier = userId,
                    DisplayName = userDisplayName
                }
            };

            return Task.CompletedTask;
        }

        private Task Leave()
        {
            this.Player = null;
            return Task.CompletedTask;
        }

        private Task ToggleReady()
        {
            if (this.Player == null)
                return Task.CompletedTask;

            this.Player.IsReady = !this.Player.IsReady;

            return Task.CompletedTask;
        }
    }

    internal class Player
    {
        public required User User { get; init; }

        public required bool IsReady { get; set; }
    }

    internal class User
    {
        public required string? Identifier { get; init; }
        public required string? DisplayName { get; init; }
    }
}
