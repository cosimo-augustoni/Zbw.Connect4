using Game.Contract;
using Game.Contract.Queries;
using Game.Domain.GameProjections;
using Shared.Application;

namespace Game.Application.Queries
{
    internal class AllGameLobbiesQueryHandler(IGameLobbiesQuery gameLobbiesQuery) : IQueryHandler<AllGameLobbiesQuery, IReadOnlyList<GameLobbyDto>>
    {
        public async Task<IReadOnlyList<GameLobbyDto>> Handle(AllGameLobbiesQuery request, CancellationToken cancellationToken)
        {
            var gameLobbies = await gameLobbiesQuery.GetAllLobbies();

            return gameLobbies
                .Select(l => new GameLobbyDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    HasOpenPlayerSlot = l.HasOpenPlayerSlot
                })
                .ToList();
        }
    }
}
