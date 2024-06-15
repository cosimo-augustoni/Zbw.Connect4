using Game.Contract;
using Game.Contract.Queries.Dtos;
using PlayerClient.Contract;
using System.Security.Cryptography;
using System.Text;
using PlayerClient.AI;

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

                if (this.Player.Owner.Identifier == AiPlayerClientConstants.EasyPlayerClientType.Identifier)
                    return $"ais/AI_Easy.jpeg";
                if (this.Player.Owner.Identifier == AiPlayerClientConstants.MediumPlayerClientType.Identifier)
                    return $"ais/AI_Medium.jpeg";
                if (this.Player.Owner.Identifier == AiPlayerClientConstants.HardPlayerClientType.Identifier)
                    return $"ais/AI_Hard.jpeg";

                using var sha = MD5.Create();
                var imageCount = 30;
                var hashedId = sha.ComputeHash(Encoding.UTF8.GetBytes(this.Player.Owner.Identifier));
                int imageId = Math.Abs(BitConverter.ToInt32(hashedId, 0)) % imageCount + 1;
                return $"avatars/{imageId:D2}.png";
            }
        }
    }
}