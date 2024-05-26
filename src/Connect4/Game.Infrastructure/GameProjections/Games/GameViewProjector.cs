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
            INotificationHandler<PlayerReadiedEvent>,
            INotificationHandler<PlayerUnreadiedEvent>,
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
                YellowPlayer = null,
                RedPlayer = null,
                CurrentPlayerId = null,
                IsRunning = false,
                IsFinished = false,
                FinishReason = null,
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
            var player = new PlayerViewDbo
            {
                PlayerId = notification.Player.Id.Id,
                Owner = new PlayerOwnerDbo
                {
                    Identifier = notification.Player.PlayerOwner.Identifier,
                    DisplayName = notification.Player.PlayerOwner.DisplayName,
                },
                IsReady = false,
                Type = notification.Player.TypeIdentifier
            };

            var updateDefinition = notification.PlayerSide == PlayerSide.Red
                ? Builders<GameViewDbo>.Update.Set(g => g.RedPlayer, player) 
                : Builders<GameViewDbo>.Update.Set(g => g.YellowPlayer, player);

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(PlayerRemovedEvent notification, CancellationToken cancellationToken)
        {
            var game = this.GetGameById(notification.GameId);

            var updateDefinition = game.RedPlayer?.PlayerId == notification.PlayerId.Id
                ? Builders<GameViewDbo>.Update.Set(g => g.RedPlayer, null) 
                : Builders<GameViewDbo>.Update.Set(g => g.YellowPlayer, null);

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(PlayerReadiedEvent notification, CancellationToken cancellationToken)
        {
            var game = this.GetGameById(notification.GameId);

            var updateDefinition = game.RedPlayer?.PlayerId == notification.PlayerId.Id
                ? Builders<GameViewDbo>.Update.Set(g => g.RedPlayer!.IsReady, true) 
                : Builders<GameViewDbo>.Update.Set(g => g.YellowPlayer!.IsReady, true);

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(PlayerUnreadiedEvent notification, CancellationToken cancellationToken)
        {
            var game = this.GetGameById(notification.GameId);

            var updateDefinition = game.RedPlayer?.PlayerId == notification.PlayerId.Id
                ? Builders<GameViewDbo>.Update.Set(g => g.RedPlayer!.IsReady, false) 
                : Builders<GameViewDbo>.Update.Set(g => g.YellowPlayer!.IsReady, false);

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
            game.Board.PlacePiece(notification.Position, game.CurrentPlayerId == game.YellowPlayer?.PlayerId ? PlayerSide.Yellow : PlayerSide.Red);

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
                .Set(g => g.CurrentPlayerId, game.CurrentPlayerId == game.YellowPlayer?.PlayerId ? game.RedPlayer?.PlayerId : game.YellowPlayer?.PlayerId);

            await this.UpdateInternalAsync(notification, cancellationToken, updateDefinition);
        }

        public async Task Handle(GameFinishedEvent notification, CancellationToken cancellationToken)
        {
            var updateDefinition = Builders<GameViewDbo>.Update.Set(g => g.IsFinished, true);

            var finishReason = notification.FinishReason switch
            {
                FinishReason.Win => "Gewonnen",
                FinishReason.Draw => "Unentschieden",
                _ => throw new ArgumentOutOfRangeException()
            };
            updateDefinition = updateDefinition.Set(g => g.FinishReason, finishReason);

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
