using Game.Domain;
using Game.Domain.GameAggregate;
using Orleans;

namespace Game.Infrastructure.GameAggregate
{
    public class GameRepository(IGrainFactory grainFactory) : IGameRepository
    {
        public IGame Create()
        {
            return grainFactory.GetGrain<IGameActor>(new GameId().Id);
        }

        public IGame GetById(GameId gameId)
        {
            return grainFactory.GetGrain<IGameActor>(gameId.Id);
        }
    }
}