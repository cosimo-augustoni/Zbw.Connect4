using Game.Domain.GameAggregate;
using Orleans;

namespace Game.Infrastructure.GameAggregate
{
    public interface IGameActor : IGame, IGrainWithGuidKey;
}