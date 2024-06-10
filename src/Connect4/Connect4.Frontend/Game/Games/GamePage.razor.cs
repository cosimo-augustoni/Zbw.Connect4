using Game.Contract;
using Game.Contract.Queries;
using Game.Contract.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Components;
using PlayerClient.Contract.Queries;
using PlayerClient.Contract;

namespace Connect4.Frontend.Game.Games
{
    public partial class GamePage : IDisposable
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private GameChangedEventHandler GameChangedEventHandler { get; set; } = null!;

        [Inject]
        private PlayerClientChangedEventHandler PlayerClientChangedEventHandler { get; set; } = null!;

        [Parameter]
        public Guid GameId { get; set; }

        private GameDto? Game { get; set; }

        public string? FinishReason
        {
            get
            {
                if(this.Game == null || !this.Game.IsFinished)
                    return String.Empty;

                if (this.Game.WinningPlayerId == null)
                    return this.Game?.FinishReason;

                if (this.Game.WinningPlayerId == this.YellowPlayer.Player?.Id)
                    return $"{this.Game.FinishReason} von {this.YellowPlayer.Player.Owner.DisplayName}";

                if (this.Game.WinningPlayerId == this.RedPlayer.Player?.Id)
                    return $"{this.Game.FinishReason} von {this.RedPlayer.Player.Owner.DisplayName}";

                return String.Empty;
            }
        }

        private PlayerUIClient YellowPlayer { get; set; } = new PlayerUIClient
        {
            PlayerSide = PlayerSide.Yellow,
            PlayerClient = null,
            Player = null
        };
        private PlayerUIClient RedPlayer { get; set; } = new PlayerUIClient
        {
            PlayerSide = PlayerSide.Red,
            PlayerClient = null,
            Player = null
        };


        protected override async Task OnInitializedAsync()
        {
            await this.LoadGame();
            this.GameChangedEventHandler.GameChanged += this.OnGameChanged;
            this.PlayerClientChangedEventHandler.PlayerClientCreated += this.OnPlayerClientChanged;
            this.PlayerClientChangedEventHandler.PlayerClientDeleted += this.OnPlayerClientChanged;
            await base.OnInitializedAsync();
        }

        private async Task LoadGame()
        {
            this.Game = await this.Mediator.Send(new GameByIdQuery { Id = new GameId(this.GameId) });

            this.YellowPlayer = this.YellowPlayer with
            {
                Player = this.Game.YellowPlayer
            };
            if (this.YellowPlayer.Player != null)
                await this.LoadPlayerClient(this.YellowPlayer.Player.Id);

            this.RedPlayer = this.RedPlayer with
            {
                Player = this.Game.RedPlayer
            };
            if (this.RedPlayer.Player != null)
                await this.LoadPlayerClient(this.RedPlayer.Player.Id);
        }

        private async Task OnPlayerClientChanged(object sender, PlayerClientChangedEventArgs e)
        {
            await this.LoadPlayerClient(e.PlayerId);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task LoadPlayerClient(PlayerId playerId)
        {
            if (this.YellowPlayer.Player != null && this.YellowPlayer.Player.Id == playerId)
            {
                this.YellowPlayer = this.YellowPlayer with
                {
                    PlayerClient = await this.Mediator.Send(new PlayerClientByPlayerQuery
                    {
                        PlayerId = this.YellowPlayer.Player.Id,
                        PlayerClientType = new PlayerClientType(this.YellowPlayer.Player.Type)
                    }),
                };
            }
            else if (this.RedPlayer.Player != null && this.RedPlayer.Player.Id == playerId)
            {
                this.RedPlayer = this.RedPlayer with
                {
                    PlayerClient = await this.Mediator.Send(new PlayerClientByPlayerQuery
                    {
                        PlayerId = this.RedPlayer.Player.Id,
                        PlayerClientType = new PlayerClientType(this.RedPlayer.Player.Type)
                    }),
                };
            }
        }

        private async Task OnGameChanged(object sender, GameChangedEventArgs e)
        {
            if (e.GameId != this.Game?.Id)
                return;

            await this.LoadGame();
            await this.InvokeAsync(this.StateHasChanged);
        }

        

        private void CloseGame()
        {
            this.NavigationManager.NavigateTo("");
        }

        public void Dispose()
        {
            this.GameChangedEventHandler.GameChanged -= this.OnGameChanged;
            this.PlayerClientChangedEventHandler.PlayerClientCreated -= this.OnPlayerClientChanged;
            this.PlayerClientChangedEventHandler.PlayerClientDeleted -= this.OnPlayerClientChanged;
        }


    }
}
