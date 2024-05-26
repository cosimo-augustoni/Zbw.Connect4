using Microsoft.AspNetCore.Components;
using Game.Contract.Queries;
using System.Security.Cryptography;

namespace Connect4.Frontend.Game.Lobbies
{
    public partial class GameLobbyDisplay
    {
        [Parameter]
        public required GameLobbyDto Lobby { get; init; }

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        public string LobbyImage
        {
            get
            {
                using var sha = MD5.Create();
                var imageCount = 4;
                var hashedGuid = sha.ComputeHash(this.Lobby.Id.Id.ToByteArray());
                int imageId = Math.Abs(BitConverter.ToInt32(hashedGuid, 0)) % imageCount + 1;
                return $"{imageId}.jpeg";
            }
        }

        public void JoinGame()
        {
            this.NavigationManager.NavigateTo($"game/{this.Lobby.Id.Id}");
        }
    }
}
