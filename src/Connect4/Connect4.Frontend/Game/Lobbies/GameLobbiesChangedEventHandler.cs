using Connect4.Frontend.Shared;
using Game.Contract;
using Game.Contract.Queries.Notifications;
using MediatR;

namespace Connect4.Frontend.Game.Lobbies
{
    internal class GameLobbiesChangedEventHandler
    {
        public event AsyncEventHandler<GameLobbyChangedEventArgs>? GameLobbyUpdated;
        public event AsyncEventHandler<GameLobbyChangedEventArgs>? GameLobbyCreated;
        public event AsyncEventHandler<GameLobbyChangedEventArgs>? GameLobbyDeleted;

        internal async Task NotifyUpdatedAsync(GameId id)
        {
            if (this.GameLobbyUpdated != null)
                await this.GameLobbyUpdated.Invoke(this, new GameLobbyChangedEventArgs { GameId = id });
        }

        internal async Task NotifyCreatedAsync(GameId id)
        {
            if (this.GameLobbyCreated != null)
                await this.GameLobbyCreated.Invoke(this, new GameLobbyChangedEventArgs { GameId = id });
        }

        internal async Task NotifyDeletedAsync(GameId id)
        {
            if (this.GameLobbyDeleted != null)
                await this.GameLobbyDeleted.Invoke(this, new GameLobbyChangedEventArgs { GameId = id });
        }

    }

    internal class GameLobbiesChangedNotificationHandler(GameLobbiesChangedEventHandler gameLobbiesChangedEventHandler) 
        : INotificationHandler<GameLobbyUpdatedNotification>,
            INotificationHandler<GameLobbyCreatedNotification>,
            INotificationHandler<GameLobbyDeletedNotification>
    {
        public async Task Handle(GameLobbyUpdatedNotification notification, CancellationToken cancellationToken)
        {
            await gameLobbiesChangedEventHandler.NotifyUpdatedAsync(notification.GameId);
        }

        public async Task Handle(GameLobbyCreatedNotification notification, CancellationToken cancellationToken)
        {
            await gameLobbiesChangedEventHandler.NotifyCreatedAsync(notification.GameId);
        }

        public async Task Handle(GameLobbyDeletedNotification notification, CancellationToken cancellationToken)
        {
            await gameLobbiesChangedEventHandler.NotifyDeletedAsync(notification.GameId);
        }
    }

    internal class GameLobbyChangedEventArgs : EventArgs
    {
        internal required GameId GameId { get; init; }
    }
}
