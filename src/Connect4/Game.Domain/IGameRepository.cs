namespace Game.Domain
{
    public interface IGameRepository
    {
        public IGame Create();

        public IGame GetById(GameId gameId);
    }
}
