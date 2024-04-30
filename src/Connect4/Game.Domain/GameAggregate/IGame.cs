namespace Game.Domain.GameAggregate
{
    public interface IGame
    {
        Task<Guid> CreateGame();
        Task UpdateGameNameAsync(string name);
    }
}
