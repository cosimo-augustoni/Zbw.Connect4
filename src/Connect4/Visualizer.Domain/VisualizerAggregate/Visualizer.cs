using Game.Contract;
using Shared.Domain;
using Visualizer.Contract;
using Visualizer.Contract.Events;

namespace Visualizer.Domain.VisualizerAggregate
{
    public class Visualizer(VisualizerId id, IEventRegistry<VisualizerEvent> eventRegistry, TimeProvider timeProvider) : AggregateRoot<VisualizerEvent>(eventRegistry), IVisualizer
    {
        public Visualizer(
            VisualizerId id, 
            string? name, 
            string? externalId, 
            VisualizerStatus? status, 
            DateTimeOffset? deletedAt, 
            IEventRegistry<VisualizerEvent> eventRegistry, 
            TimeProvider timeProvider) 
            : this(id, eventRegistry, timeProvider)
        {
            this.Name = name;
            this.ExternalId = externalId;
            this.Status = status ?? VisualizerStatus.NotInitialized;
            this.DeletedAt = deletedAt;
        }

        public VisualizerId Id { get; } = id;

        public string? Name { get; private set; }

        public string? ExternalId { get; private set; }

        public VisualizerStatus Status { get; private set; } = VisualizerStatus.NotInitialized;

        public DateTimeOffset? DeletedAt { get; private set; }

        public GameId? CurrentGameId { get; private set; }

        private bool IsDeleted => this.DeletedAt != null;

        public async Task<Guid> CreateVisualizer(string name, string externalId)
        {
            if (this.IsDeleted)
                throw new VisualizerDeletedException();

            await this.RaiseEventAsync(new VisualizerCreatedEvent
            {
                VisualizerId = this.Id
            });

            await this.ChangeNameAsync(name);
            await this.ChangeExternalIdAsync(externalId);
            await this.ChangeStatusAsync(VisualizerStatus.Unknown);

            return this.Id.Id;
        }

        public async Task ChangeNameAsync(string name)
        {
            if (this.IsDeleted)
                throw new VisualizerDeletedException();

            await this.RaiseEventAsync(new VisualizerNameChangedEvent
            {
                VisualizerId = this.Id,
                Name = name
            });
        }

        public async Task ChangeStatusAsync(VisualizerStatus status)
        {
            if (this.Status.Id == status.Id)
                return;

            if (this.IsDeleted)
                throw new VisualizerDeletedException();

            await this.RaiseEventAsync(new VisualizerStatusChangedEvent
            {
                VisualizerId = this.Id,
                Status = status
            });
        }

        public async Task ChangeExternalIdAsync(string externalId)
        {
            if (this.IsDeleted)
                throw new VisualizerDeletedException();

            await this.RaiseEventAsync(new VisualizerExternalIdChangedEvent()
            {
                VisualizerId = this.Id,
                PreviousExternalId = this.ExternalId,
                ExternalId = externalId
            });
        }

        public async Task DeleteAsync()
        {
            if (this.IsDeleted)
                throw new VisualizerDeletedException();

            await this.RaiseEventAsync(new VisualizerDeletedEvent()
            {
                VisualizerId = this.Id,
                DeletedAt = timeProvider.GetUtcNow(),
            });
        }

        public async Task AddToGameAsync(GameId gameId)
        {
            if (this.CurrentGameId != null)
                throw new VisualizerAlreadyInGameException();

            await this.RaiseEventAsync(new VisualizerAddedToGameEvent()
            {
                VisualizerId = this.Id,
                GameId = gameId,
            });
        }

        public async Task RemoveFromGameAsync()
        {
            if (this.CurrentGameId == null)
                return;

            await this.RaiseEventAsync(new VisualizerRemovedFromGameEvent()
            {
                VisualizerId = this.Id,
            });
        }

        public override void Apply(VisualizerEvent @event)
        {
            switch (@event)
            {
                case VisualizerAddedToGameEvent visualizerAddedToGameEvent:
                    this.Apply(visualizerAddedToGameEvent);
                    break;
                case VisualizerCreatedEvent:
                    break;
                case VisualizerDeletedEvent visualizerDeletedEvent:
                    this.Apply(visualizerDeletedEvent);
                    break;
                case VisualizerExternalIdChangedEvent visualizerExternalIdChangedEvent:
                    this.Apply(visualizerExternalIdChangedEvent);
                    break;
                case VisualizerNameChangedEvent visualizerNameChangedEvent:
                    this.Apply(visualizerNameChangedEvent);
                    break;
                case VisualizerRemovedFromGameEvent visualizerRemovedFromGameEvent:
                    this.Apply(visualizerRemovedFromGameEvent);
                    break;
                case VisualizerStatusChangedEvent visualizerStatusChangedEvent:
                    this.Apply(visualizerStatusChangedEvent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@event));
            }
        }

        public void Apply(VisualizerDeletedEvent @event)
        {
            this.DeletedAt = @event.DeletedAt;
        }

        public void Apply(VisualizerExternalIdChangedEvent @event)
        {
            this.ExternalId = @event.ExternalId;
        }

        public void Apply(VisualizerNameChangedEvent @event)
        {
            this.Name = @event.Name;
        }

        public void Apply(VisualizerStatusChangedEvent @event)
        {
            this.Status = @event.Status;
        }

        public void Apply(VisualizerAddedToGameEvent @event)
        {
            this.CurrentGameId = @event.GameId;
        }

        public void Apply(VisualizerRemovedFromGameEvent @event)
        {
            this.CurrentGameId = null;
        }
    }
}
