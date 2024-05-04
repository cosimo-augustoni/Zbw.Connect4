namespace Game.Domain.GameAggregate
{
    public interface IGame
    {
        Task<Guid> CreateGame();
        Task ChangeNameAsync(string name);
    }
}
