using Game.Contract;
using Game.Domain.GameProjections;
using Game.Infrastructure.GameProjections.Players;
using MongoDB.Driver;

namespace Game.Infrastructure.GameProjections.Games
{
    internal class GameQuery(IMongoDatabase database, PlayerViewQuery playerViewQuery) : IGameQuery
    {
        public async Task<GameView> GetByIdAsync(GameId id, CancellationToken cancellationToken = default)
        {
            var game = database.GetCollection<GameViewDbo>("games")
                .AsQueryable()
                .First(v => v.GameId == id.Id);

            var yellowPlayer =  game.YellowPlayerId != null
                ? await playerViewQuery.GetByIdAsync(new PlayerId(game.YellowPlayerId.Value), cancellationToken: cancellationToken)
                : null;

            var redPlayer = game.RedPlayerId != null
                ? await playerViewQuery.GetByIdAsync(new PlayerId(game.RedPlayerId.Value), cancellationToken: cancellationToken)
                : null;

            return new GameView
            {
                Id = new GameId(game.GameId),
                Name = game.Name,
                YellowPlayer = yellowPlayer != null ? new PlayerView
                {
                    Id = new PlayerId(yellowPlayer.PlayerId),
                    Name = yellowPlayer.Name,
                    IsReady = yellowPlayer.IsReady
                } : null,
                RedPlayer = redPlayer != null ? new PlayerView
                {
                    Id = new PlayerId(redPlayer.PlayerId),
                    Name = redPlayer.Name,
                    IsReady = redPlayer.IsReady
                } : null,
                CurrentPlayerId = game.CurrentPlayerId != null ? new PlayerId(game.CurrentPlayerId.Value) : null,
                WinningPlayerId = game.WinningPlayerId != null ? new PlayerId(game.WinningPlayerId.Value) : null,
                IsFinished = game.IsFinished,
                IsAborted = game.IsAborted,
                IsRunning = game.IsRunning,
                Board = game.Board
            };
        }
    }
}
