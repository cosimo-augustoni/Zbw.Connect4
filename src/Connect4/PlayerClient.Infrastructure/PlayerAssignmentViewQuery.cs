using Game.Contract;
using MongoDB.Driver;
using PlayerClient.Domain;

namespace PlayerClient.Infrastructure
{
    internal class PlayerAssignmentViewQuery(IMongoDatabase database) : IPlayerAssignmentQuery
    {
        public Task<GameId?> GetGameIdByPlayerAsync(PlayerId playerId)
        {
            var gameId = database.GetCollection<PlayerAssignmentViewDbo>("player_assignments")
                .AsQueryable()
                .FirstOrDefault(v => v.PlayerId == playerId.Id);

            return Task.FromResult(gameId != null ? new GameId(gameId.GameId) : null);
        }

        public Task<List<(PlayerId PlayerId, string PlayerType)>> GetPlayersByGameAsync(GameId gameId)
        {
            var gameAssignments = database.GetCollection<PlayerAssignmentViewDbo>("player_assignments")
                .AsQueryable()
                .Where(v => v.GameId == gameId.Id)
                .ToList();

            var result = gameAssignments.Select(a => (new PlayerId(a.PlayerId), a.PlayerType)).ToList();
            return Task.FromResult(result);
        }
    }
}
