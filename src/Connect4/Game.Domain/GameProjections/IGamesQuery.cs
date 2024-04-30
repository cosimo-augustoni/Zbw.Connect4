namespace Game.Domain.GameProjections
{
    public interface IGamesQuery
    {
        Task<IReadOnlyList<GameSummary>> GetAllGames();
    }
}
