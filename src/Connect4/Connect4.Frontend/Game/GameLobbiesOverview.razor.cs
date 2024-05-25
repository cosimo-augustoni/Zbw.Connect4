using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4.Frontend.Shared;
using Game.Contract.Queries;
using MediatR;
using Microsoft.AspNetCore.Components;
using Visualizer.Contract.Queries;

namespace Connect4.Frontend.Game
{
    public partial class GameLobbiesOverview : IDisposable
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        private GameLobbiesChangedEventHandler GameLobbiesChangedEventHandler { get; set; } = null!;

        private bool IsLoading { get; set; }

        private IReadOnlyList<GameLobbyDto> Lobbies { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            await this.LoadLobbies();
            this.GameLobbiesChangedEventHandler.GameLobbyCreated += this.OnGameLobbyCreated;
            this.GameLobbiesChangedEventHandler.GameLobbyDeleted += this.OnGameLobbyDeleted;
            this.GameLobbiesChangedEventHandler.GameLobbyUpdated += this.OnGameLobbyUpdated;
            await base.OnInitializedAsync();
        }

        private async Task OnGameLobbyCreated(object sender, GameLobbyChangedEventArgs e)
        {
            await this.LoadLobbies();
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task OnGameLobbyDeleted(object sender, GameLobbyChangedEventArgs e)
        {
            await this.LoadLobbies();
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task OnGameLobbyUpdated(object sender, GameLobbyChangedEventArgs e)
        {
            await this.LoadLobbies();
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task LoadLobbies()
        {
            try
            {
                this.IsLoading = true;
                this.Lobbies = await this.Mediator.Send(new AllGameLobbiesQuery());
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        public void Dispose()
        {
            this.GameLobbiesChangedEventHandler.GameLobbyCreated -= this.OnGameLobbyCreated;
            this.GameLobbiesChangedEventHandler.GameLobbyDeleted -= this.OnGameLobbyDeleted;
            this.GameLobbiesChangedEventHandler.GameLobbyUpdated -= this.OnGameLobbyUpdated;
        }
    }
}
