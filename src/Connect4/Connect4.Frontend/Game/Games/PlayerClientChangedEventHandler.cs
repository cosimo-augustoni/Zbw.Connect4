using Connect4.Frontend.Shared;
using Game.Contract;
using MediatR;
using PlayerClient.Contract.Queries;

namespace Connect4.Frontend.Game.Games
{
    internal class PlayerClientChangedEventHandler
    {
        public event AsyncEventHandler<PlayerClientChangedEventArgs>? PlayerClientCreated;
        public event AsyncEventHandler<PlayerClientChangedEventArgs>? PlayerClientDeleted;

        internal async Task NotifyCreatedAsync(GameId gameId, PlayerId playerId)
        {
            if (this.PlayerClientCreated != null)
                await this.PlayerClientCreated.Invoke(this, new PlayerClientChangedEventArgs { GameId = gameId, PlayerId = playerId});
        }

        internal async Task NotifyDeletedAsync(GameId gameId, PlayerId playerId)
        {
            if (this.PlayerClientDeleted != null)
                await this.PlayerClientDeleted.Invoke(this, new PlayerClientChangedEventArgs { GameId = gameId, PlayerId = playerId});
        }
    }

    internal class PlayerClientChangedNotificationHandler(PlayerClientChangedEventHandler playerClientChangedEventHandler) 
        : INotificationHandler<PlayerClientCreatedNotification>,
            INotificationHandler<PlayerClientDeletedNotification>
    {
        public async Task Handle(PlayerClientCreatedNotification notification, CancellationToken cancellationToken)
        {
            await playerClientChangedEventHandler.NotifyCreatedAsync(notification.GameId, notification.PlayerId);
        }

        public async Task Handle(PlayerClientDeletedNotification notification, CancellationToken cancellationToken)
        {
            await playerClientChangedEventHandler.NotifyDeletedAsync(notification.GameId, notification.PlayerId);
        }
    }

    internal class PlayerClientChangedEventArgs : EventArgs
    {
        internal required GameId GameId { get; init; }
        internal required PlayerId PlayerId { get; init; }
    }
}