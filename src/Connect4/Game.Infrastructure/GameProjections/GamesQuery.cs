using Game.Domain.GameProjections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Domain;
using MongoDB.Driver;

namespace Game.Infrastructure.GameProjections
{
    internal class GamesQuery(IMongoDatabase database) : IGamesQuery
    {
        public async Task<IReadOnlyList<GameSummary>> GetAllGames()
        {
            var games = await database.GetCollection<GameSummaryDbo>("game_summaries").AsQueryable().ToListAsync();
            return games.Select(g => new GameSummary
            {
                Id = new GameId(g.GameId),
                Name = g.Name,
            }).ToList();
        }
    }
}
