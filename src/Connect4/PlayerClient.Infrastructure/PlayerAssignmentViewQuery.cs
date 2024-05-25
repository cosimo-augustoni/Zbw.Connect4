using MongoDB.Driver;
using PlayerClient.Contract;
using PlayerClient.Domain;

namespace PlayerClient.Infrastructure
{
    internal class PlayerAssignmentViewQuery(IMongoDatabase database) : IPlayerAssignmentQuery
    {
        public Task<GameId> GetGameIdByPlayerAsync(PlayerId playerId)
        {
            var gameId = database.GetCollection<PlayerAssignmentViewDbo>("player_assignments")
                .AsQueryable()
                .First(v => v.PlayerId == playerId.Id)
                .GameId;

            return Task.FromResult(new GameId(gameId));
        }
    }
}
