namespace Game.Domain.GameAggregate
{
    public interface IGameRepository
    {
        public IGame Create();

        public IGame GetById(GameId gameId);
    }
}
