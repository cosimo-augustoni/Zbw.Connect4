using Connect4.Frontend.Shared;
using Game.Contract;
using Game.Contract.Queries.Notifications;
using MediatR;

namespace Connect4.Frontend.Game
{
    internal class GameChangedEventHandler
    {
        public event AsyncEventHandler<GameChangedEventArgs>? GameChanged;

        internal async Task NotifyUpdatedAsync(GameId id)
        {
            if (this.GameChanged != null)
                await this.GameChanged.Invoke(this, new GameChangedEventArgs { GameId = id });
        }
    }

    internal class GameChangedNotificationHandler(GameChangedEventHandler gameChangedEventHandler) 
        : INotificationHandler<GameUpdatedNotification>
    {
        public async Task Handle(GameUpdatedNotification notification, CancellationToken cancellationToken)
        {
            await gameChangedEventHandler.NotifyUpdatedAsync(notification.GameId);
        }
    }

    internal class GameChangedEventArgs : EventArgs
    {
        internal required GameId GameId { get; init; }
    }
}
