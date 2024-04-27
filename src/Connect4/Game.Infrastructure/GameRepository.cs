using System.Diagnostics.CodeAnalysis;
using Game.Domain;
using Orleans;
using Orleans.EventSourcing;
using Orleans.Providers;
using Shared.Infrastructure;

namespace Game.Infrastructure
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

    [StorageProvider(ProviderName = "games")]
    [LogConsistencyProvider(ProviderName = "LogStorage")]
    public class GameActor : JournaledGrain<GameState, GameEvent>, IGameActor
    {
        public ValueTask<Guid> GetId() => new(this.GetPrimaryKey());

        private readonly EventRegistry<GameEvent> eventRegistry = new();

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            this.State.Game = new Domain.Game(new GameId(await this.GetId()), this.eventRegistry);
            this.State.IsInitialised = true;

            await base.OnActivateAsync(cancellationToken);
        }

        public async Task UpdateGameNameAsync(string name)
        {
            if (!this.State.IsInitialised)
                throw new GrainUninitialisedOnAccessException();

            await this.State.Game.UpdateGameNameAsync(name);

            this.RaiseEvents(this.eventRegistry.Events);
            await this.ConfirmEvents();
        }

        protected override void TransitionState(GameState state, GameEvent @event)
        {
            if (!state.IsInitialised)
                throw new GrainUninitialisedOnAccessException();

            state.Game.Apply(@event);
        }
    }

    public class GameState
    {
        [MemberNotNullWhen(true, nameof(Game))]
        public bool IsInitialised { get; set; }
        
        public Domain.Game? Game { get; set; }
    }

    public interface IGameActor : IGame, IGrainWithGuidKey;
}