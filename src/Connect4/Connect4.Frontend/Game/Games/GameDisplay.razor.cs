using Game.Contract.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace Connect4.Frontend.Game.Games
{
    public partial class GameDisplay
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
    }
}
