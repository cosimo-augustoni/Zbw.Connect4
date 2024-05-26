using Game.Contract;
using MediatR;

namespace PlayerClient.Contract.Queries
{
    public class PlayerClientDeletedNotification : INotification
    {
        public required GameId GameId { get; init; }

        public required PlayerId PlayerId { get; init; }
    }
}