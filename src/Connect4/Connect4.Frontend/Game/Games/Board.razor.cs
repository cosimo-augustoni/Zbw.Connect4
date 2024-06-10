using System.Runtime.CompilerServices;
using Game.Contract;
using Game.Contract.Commands;
using Game.Contract.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;

namespace Connect4.Frontend.Game.Games
{
    public partial class Board
    {
        [Parameter] public required BoardDto BoardState { get; set; }

        [Parameter] public required PlayerUIClient CurrentPlayer { get; set; }

        [Parameter] public required GameId GameId { get; set; }

        [Inject] private ISender Mediator { get; set; } = null!;

        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        private bool IsBoardReadOnly { get; set; }

        private bool IsPiecePlacing { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.EvaluateBoardReadOnlyAsync();
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            this.IsPiecePlacing = false;
            await this.EvaluateBoardReadOnlyAsync();
            await base.OnParametersSetAsync();
        }

        //TODO Visualizer Status berücksichtigen
        private async Task EvaluateBoardReadOnlyAsync()
        {
            var authenticationState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
            var isMyPlayer = this.CurrentPlayer.Player?.Owner.Identifier == authenticationState.User.GetNameIdentifierId();
            this.IsBoardReadOnly = !isMyPlayer || this.IsPiecePlacing;
        }

        private async Task CellClickedAsync(int xCoord)
        {
            this.IsPiecePlacing = true;
            var yCoord = 0;
            for (var row = 0; row < 6; row++)
            {
                if (this.BoardState.BoardState[0][xCoord].SlotState != SlotState.Empty)
                    return;

                if (this.BoardState.BoardState[row][xCoord].SlotState == SlotState.Empty)
                    yCoord = row;
                else
                    break;
            }

            var boardPosition = new BoardPosition(xCoord, yCoord);
            await this.Mediator.Send(new PlaceGamePieceCommand(this.GameId, boardPosition));
        }
    }
}