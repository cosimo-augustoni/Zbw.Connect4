using Game.Domain;
using Orleans;

namespace Game.Infrastructure
{
    public interface IGameActor : IGame, IGrainWithGuidKey;
}