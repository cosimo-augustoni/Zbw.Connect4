using MediatR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Contract.Commands;

namespace Connect4.Frontend.Game
{
    public partial class GameCreationButtons
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        private bool IsCreating { get; set; }

        private async Task CreateGame()
        {
            Guid gameId;
            try
            {
                this.IsCreating = true;
                gameId = await this.Mediator.Send(new CreateGameCommand());
            }
            finally
            {
                this.IsCreating = false;
            }
            this.NavigationManager.NavigateTo($"game/{gameId}");
        }
    }
}
