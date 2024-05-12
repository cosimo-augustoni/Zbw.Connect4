using Game.Contract;
using Game.Contract.Events;
using Game.Contract.Queries.Notifications;
using MediatR;
using MongoDB.Driver;

namespace Game.Infrastructure.GameProjections.Games
{
    internal class GameViewProjector(IMongoDatabase database, IPublisher notificationPublisher)
        : INotificationHandler<GameCreatedEvent>,
            INotificationHandler<GameNameChangedEvent>,
            INotificationHandler<PlayerAddedEvent>,
            INotificationHandler<PlayerRemovedEvent>,
            INotificationHandler<GamePiecePlacementRequestedEvent>,
            INotificationHandler<GamePiecePlacementRejectedEvent>,
            INotificationHandler<GamePiecePlacedEvent>,
            INotificationHandler<GameFinishedEvent>,
            INotificationHandler<GameAbortedEvent>,
            INotificationHandler<GameStartedEvent>
    {
        public async Task Handle(GameCreatedEvent notification, CancellationToken cancellationToken)
        {
            var gameView = new GameViewDbo()
            {
                GameId = notification.GameId.Id,
                Name = notification.Name,
                Board = new Board(),
                YellowPlayerId = null,
                RedPlayerId = null,
                CurrentPlayerId = null,
                IsRunning = false,
                IsFinished = false,
                WinningPlayerId = null,
                IsAborted = false,
            };
            await this.GetGamesCollection().InsertOneAsync(gameView, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new GameCreatedNotification { GameId = notification.GameId }, cancellationToken);
        }

        public async Task Handle(GameNameChangedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = Builders<GameViewDbo>.Update.Set(g => g.Name, notification.Name);
            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(PlayerAddedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = notification.PlayerSide == PlayerSide.Red
                ? Builders<GameViewDbo>.Update.Set(g => g.RedPlayerId, notification.Player.Id.Id) 
                : Builders<GameViewDbo>.Update.Set(g => g.YellowPlayerId, notification.Player.Id.Id);

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(PlayerRemovedEvent notification, CancellationToken cancellationToken)
        {
            var game = this.GetGameById(notification.GameId);

            var updateDefinition = game.RedPlayerId == notification.PlayerId.Id
                ? Builders<GameViewDbo>.Update.Set(g => g.RedPlayerId, null) 
                : Builders<GameViewDbo>.Update.Set(g => g.YellowPlayerId, null);

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(GameStartedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = Builders<GameViewDbo>.Update
                .Set(g => g.IsRunning, true)
                .Set(g => g.CurrentPlayerId, notification.StartingPlayerId.Id);

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(GamePiecePlacementRequestedEvent notification, CancellationToken cancellationToken)
        {
            var game = this.GetGameById(notification.GameId);
            game.Board.PlacePiece(notification.Position, game.CurrentPlayerId == game.YellowPlayerId ? PlayerSide.Yellow : PlayerSide.Red);

            var updateDefinition = Builders<GameViewDbo>.Update
                .Set(g => g.Board, game.Board);

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(GamePiecePlacementRejectedEvent notification, CancellationToken cancellationToken)
        {
            var game = this.GetGameById(notification.GameId);
            game.Board.RemovePiece(notification.Position);

            var updateDefinition = Builders<GameViewDbo>.Update
                .Set(g => g.Board, game.Board);

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(GamePiecePlacedEvent notification, CancellationToken cancellationToken)
        {
            var game = this.GetGameById(notification.GameId);
            var updateDefinition = Builders<GameViewDbo>.Update
                .Set(g => g.CurrentPlayerId, game.CurrentPlayerId == game.YellowPlayerId ? game.RedPlayerId : game.YellowPlayerId);

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(GameFinishedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = Builders<GameViewDbo>.Update.Set(g => g.IsFinished, true);
            if (notification.FinishReason == FinishReason.Win)
            {
                updateDefinition = updateDefinition.Set(g => g.WinningPlayerId, notification.WinningPlayerId?.Id);
            }

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(GameAbortedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = Builders<GameViewDbo>.Update.Set(g => g.IsAborted, true);
            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        private IMongoCollection<GameViewDbo> GetGamesCollection() => database.GetCollection<GameViewDbo>("games");

        private async Task UpdateInternalAsync(GameEvent notification, CancellationToken cancellationToken, UpdateDefinition<GameViewDbo> updateDefinition)
        {
            await this.GetGamesCollection()
                .FindOneAndUpdateAsync(g => g.GameId == notification.GameId.Id, updateDefinition, cancellationToken: cancellationToken);

            await notificationPublisher.Publish(new GameUpdatedNotification { GameId = notification.GameId }, cancellationToken);
        }

        private GameViewDbo GetGameById(GameId gameId)
        {
            return this.GetGamesCollection().AsQueryable().First(g => g.GameId == gameId.Id);
        }
    }
}
