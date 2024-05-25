using Game.Contract.Queries.Dtos;
using Shared.Contract;

namespace Game.Contract.Queries
{
    public class GameByIdQuery : IQuery<GameDto>
    {
        public required GameId Id { get; init; }
    }
}