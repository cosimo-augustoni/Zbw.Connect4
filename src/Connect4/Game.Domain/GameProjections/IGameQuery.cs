using Game.Contract;

namespace Game.Domain.GameProjections
{
    public interface IGameQuery
    {
        Task<GameView> GetByIdAsync(GameId id, CancellationToken cancellationToken = default);
    }
}
