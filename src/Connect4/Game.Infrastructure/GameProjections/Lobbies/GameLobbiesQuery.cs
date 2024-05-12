using Game.Contract;
using Game.Domain.GameProjections;
using MongoDB.Driver;

namespace Game.Infrastructure.GameProjections.Lobbies
{
    internal class GameLobbiesQuery(IMongoDatabase database) : IGameLobbiesQuery
    {
        public async Task<IReadOnlyList<GameLobby>> GetAllLobbies()
        {
            var games = await database.GetCollection<GameLobbyDbo>("game_summaries").AsQueryable().ToListAsync();
            return games.Select(g => new GameLobby
            {
                Id = new GameId(g.GameId),
                Name = g.Name,
                HasOpenPlayerSlot = g.OpenPlayerSlots > 0
            }).ToList();
        }
    }
}
