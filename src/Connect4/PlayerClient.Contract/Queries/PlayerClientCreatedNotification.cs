using Game.Contract;
using MediatR;

namespace PlayerClient.Contract.Queries
{
    public class PlayerClientCreatedNotification : INotification
    {
        public required GameId GameId { get; init; }

        public required PlayerId PlayerId { get; init; }
    }
}
