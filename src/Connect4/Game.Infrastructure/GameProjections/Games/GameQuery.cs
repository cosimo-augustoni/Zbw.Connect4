using Game.Contract;
using Game.Contract.Events;
using Game.Domain.GameProjections;
using MongoDB.Driver;

namespace Game.Infrastructure.GameProjections.Games
{
    internal class GameQuery(IMongoDatabase database) : IGameQuery
    {
        public Task<GameView> GetByIdAsync(GameId id, CancellationToken cancellationToken = default)
        {
            var game = database.GetCollection<GameViewDbo>("games")
                .AsQueryable()
                .First(v => v.GameId == id.Id);

            var yellowPlayer = game.YellowPlayer;

            var redPlayer = game.RedPlayer;

            var gameView = new GameView
            {
                Id = new GameId(game.GameId),
                Name = game.Name,
                YellowPlayer = yellowPlayer != null ? new PlayerView
                {
                    Id = new PlayerId(yellowPlayer.PlayerId),
                    Owner = new PlayerOwner(yellowPlayer.Owner.Identifier, yellowPlayer.Owner.DisplayName),
                    IsReady = yellowPlayer.IsReady,
                    Type = yellowPlayer.Type
                } : null,
                RedPlayer = redPlayer != null ? new PlayerView
                {
                    Id = new PlayerId(redPlayer.PlayerId),
                    Owner = new PlayerOwner(redPlayer.Owner.Identifier, redPlayer.Owner.DisplayName),
                    IsReady = redPlayer.IsReady,
                    Type = redPlayer.Type
                } : null,
                CurrentPlayerId = game.CurrentPlayerId != null ? new PlayerId(game.CurrentPlayerId.Value) : null,
                WinningPlayerId = game.WinningPlayerId != null ? new PlayerId(game.WinningPlayerId.Value) : null,
                FinishReason = game.FinishReason,
                IsFinished = game.IsFinished,
                IsAborted = game.IsAborted,
                IsRunning = game.IsRunning,
                Board = game.Board
            };
            return Task.FromResult(gameView);
        }
    }
}
