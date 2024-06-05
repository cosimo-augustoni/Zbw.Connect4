using Game.Contract;
using Orleans;
using Orleans.Runtime;
using Shared.Infrastructure;
using Visualizer.Contract;
using Visualizer.Contract.Events;

namespace Visualizer.Infrastructure.VisualizerAggregate
{
    public class VisualizerActor : Grain, IVisualizerActor
    {
        private readonly EventRegistry<VisualizerEvent> eventRegistry = new();
        private readonly TimeProvider timeProvider;

        public VisualizerActor([PersistentState("visualizer", "visualizers")] IPersistentState<VisualizerState> visualizerState,
            IEventPublisher eventPublisher,
            TimeProvider timeProvider)
        {
            this.visualizerState = visualizerState;
            this.eventPublisher = eventPublisher;
            this.timeProvider = timeProvider;
        }

        private readonly IPersistentState<VisualizerState> visualizerState;
        private readonly IEventPublisher eventPublisher;

        private Domain.VisualizerAggregate.Visualizer? Visualizer { get; set; }

        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            var state = this.visualizerState.State;

            var visualizerId = new VisualizerId(state.Id ?? this.GetPrimaryKey());
            var visualizerStatus = state.StatusId.HasValue ? VisualizerStatus.GetById(state.StatusId.Value): null;
            this.Visualizer = new Domain.VisualizerAggregate.Visualizer(
                visualizerId,
                state.Name,
                state.ExternalId,
                visualizerStatus,
                state.DeletedAt,
                this.eventRegistry,
                this.timeProvider);

            return base.OnActivateAsync(cancellationToken);
        }

        public async Task<Guid> CreateVisualizer(string name, string externalId)
        {
            if (this.Visualizer == null)
                throw new GrainUninitialisedOnAccessException();

            var result = await this.Visualizer.CreateVisualizer(name, externalId);

            await this.SaveAsync(this.Visualizer);

            return result;
        }

        public async Task ChangeNameAsync(string name)
        {
            if (this.Visualizer == null)
                throw new GrainUninitialisedOnAccessException();

            await this.Visualizer.ChangeNameAsync(name);

            await this.SaveAsync(this.Visualizer);
        }

        public async Task ChangeExternalIdAsync(string externalId)
        {
            if (this.Visualizer == null)
                throw new GrainUninitialisedOnAccessException();

            await this.Visualizer.ChangeExternalIdAsync(externalId);

            await this.SaveAsync(this.Visualizer);
        }

        public async Task DeleteAsync()
        {
            if (this.Visualizer == null)
                throw new GrainUninitialisedOnAccessException();

            await this.Visualizer.DeleteAsync();
            
            await this.SaveAsync(this.Visualizer);
        }

        public async Task AddToGameAsync(GameId gameId)
        {
            if (this.Visualizer == null)
                throw new GrainUninitialisedOnAccessException();

            await this.Visualizer.AddToGameAsync(gameId);
            
            await this.SaveAsync(this.Visualizer);
        }

        public async Task RemoveFromGameAsync()
        {
            if (this.Visualizer == null)
                throw new GrainUninitialisedOnAccessException();

            await this.Visualizer.RemoveFromGameAsync();
            
            await this.SaveAsync(this.Visualizer);
        }

        public async Task ChangeStatusAsync(VisualizerStatus status)
        {
            if (this.Visualizer == null)
                throw new GrainUninitialisedOnAccessException();

            await this.Visualizer.ChangeStatusAsync(status);
            
            await this.SaveAsync(this.Visualizer);
        }

        private async Task SaveAsync(Domain.VisualizerAggregate.Visualizer visualizer)
        {
            foreach (var @event in this.eventRegistry.Events)
            {
                visualizer.Apply(@event);
            }
            await this.WriteStateToStorageAsync(visualizer);
            await this.eventPublisher.PublishEvents(this.eventRegistry.Events);
            this.eventRegistry.Clear();
        }

        private async Task WriteStateToStorageAsync(Domain.VisualizerAggregate.Visualizer visualizer)
        {
            this.visualizerState.State.Id = visualizer.Id.Id;
            this.visualizerState.State.Name = visualizer.Name;
            this.visualizerState.State.ExternalId = visualizer.ExternalId;
            this.visualizerState.State.StatusId = visualizer.Status.Id;
            this.visualizerState.State.DeletedAt = visualizer.DeletedAt;

            await this.visualizerState.WriteStateAsync();
        }
    }
}
