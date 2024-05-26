using Game.Contract;
using Game.Contract.Queries.Dtos;
using PlayerClient.Contract;
using System.Security.Cryptography;
using System.Text;

namespace Connect4.Frontend.Game.Games
{
    public record PlayerUIClient
    {
        public required IPlayerClient? PlayerClient { get; init; }

        public required PlayerDto? Player { get; init; }

        public required PlayerSide PlayerSide { get; init; }

        public string? PlayerImage
        {
            get
            {
                if (this.Player == null)
                    return null;

                using var sha = MD5.Create();
                var imageCount = 8;
                var hashedId = sha.ComputeHash(Encoding.UTF8.GetBytes(this.Player.Owner.Identifier));
                int imageId = Math.Abs(BitConverter.ToInt32(hashedId, 0)) % imageCount + 1;
                return $"{imageId}.png";
            }
        }
    }
}