using Game.Contract;
using MongoDB.Driver;

namespace Game.Infrastructure.GameProjections.Players
{
    internal class PlayerViewQuery(IMongoDatabase database)
    {
        public Task<PlayerViewDbo> GetByIdAsync(PlayerId id, CancellationToken cancellationToken = default)
        {
            var player = database.GetCollection<PlayerViewDbo>("players")
                .AsQueryable()
                .First(v => v.PlayerId == id.Id);

            return Task.FromResult(player);
        }
    }
}
