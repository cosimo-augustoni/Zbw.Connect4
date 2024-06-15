using MediatR;
using Microsoft.AspNetCore.Components;
using Game.Contract.Commands;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Identity.Web;
using Game.Contract;
using Game.Contract.Queries;

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

        private static Random random = new Random();

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

        private async Task MatchMake(MouseEventArgs obj)
        {
            Guid gameId;
            try
            {
                this.IsCreating = true;
                var authenticationState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
                var games = await this.Mediator.Send(new AllGameLobbiesQuery());
                var openGames = games.Where(g => g.HasOpenPlayerSlot).ToList();
                if (openGames.Count > 0)
                {
                    var randomGame = openGames[random.Next(openGames.Count)];
                    gameId = randomGame.Id.Id;
                }
                else
                {
                    gameId = await this.Mediator.Send(new CreateGameCommand { Name = authenticationState.User.GetDisplayName() });
                }
            }
            finally
            {
                this.IsCreating = false;
            }
            this.NavigationManager.NavigateTo($"game/{gameId}");
        }
    }
}
