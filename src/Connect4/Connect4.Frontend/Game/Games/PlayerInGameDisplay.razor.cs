using Microsoft.AspNetCore.Components;

namespace Connect4.Frontend.Game.Games
{
    public partial class PlayerInGameDisplay
    {
        [Parameter]
        public required PlayerUIClient PlayerClient { get; set; }

        [Parameter] 
        public bool DisplayPlayerSide { get; set; }

        [Parameter] 
        public bool IsCurrentPlayer { get; set; }
    }
}
