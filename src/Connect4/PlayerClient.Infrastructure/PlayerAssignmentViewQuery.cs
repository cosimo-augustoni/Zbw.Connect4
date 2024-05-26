using Game.Contract;
using MongoDB.Driver;
using PlayerClient.Contract;
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
    }
}
