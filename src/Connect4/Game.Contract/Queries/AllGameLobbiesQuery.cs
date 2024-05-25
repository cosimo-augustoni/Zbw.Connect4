using Shared.Contract;

namespace Game.Contract.Queries
{
    public class AllGameLobbiesQuery : IQuery<IReadOnlyList<GameLobbyDto>>;
}
