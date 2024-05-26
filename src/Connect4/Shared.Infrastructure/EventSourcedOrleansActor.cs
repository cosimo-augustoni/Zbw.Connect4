using Orleans;
using Orleans.Configuration;
using Orleans.EventSourcing;
using Orleans.EventSourcing.CustomStorage;
using Shared.Contract;
using Shared.Domain;

namespace Shared.Infrastructure
{
    public abstract class EventSourcedOrleansActor<TAggregate, TState, TEvent> : JournaledGrain<TState, TEvent>, IGrainWithGuidKey
        where TState : OrleansActorStateBase<TAggregate>, new()
        where TEvent : DomainEvent
        where TAggregate : class, IAggregateRoot<TEvent>
    {
        private readonly IEventPublisher eventPublisher;
        private readonly Func<IEventRegistry<TEvent>, Guid, TAggregate> aggregateFactory;
        private readonly EventRegistry<TEvent> eventRegistry = new();

        protected EventSourcedOrleansActor(
            IEventPublisher eventPublisher,
            Func<IEventRegistry<TEvent>, Guid, TAggregate> aggregateFactory)
        {
            this.eventPublisher = eventPublisher;
            this.aggregateFactory = aggregateFactory;
        }

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            this.State.Aggregate = this.aggregateFactory(this.eventRegistry, this.GetPrimaryKey());
            this.State.IsInitialised = true;

            await base.OnActivateAsync(cancellationToken);
        }

        protected override void TransitionState(TState state, TEvent @event)
        {
            if (!state.IsInitialised)
                throw new GrainUninitialisedOnAccessException();

            state.Aggregate.Apply(@event);
        }

        protected async Task ExecuteAsync(Func<TAggregate, Task> action)
        {
            if (!this.State.IsInitialised)
                throw new GrainUninitialisedOnAccessException();

            await action(this.State.Aggregate);

            this.RaiseEvents(this.eventRegistry.Events);
            await this.ConfirmEvents();
            await this.eventPublisher.PublishEvents(this.eventRegistry.Events);
            this.eventRegistry.Clear();
        }

        protected async Task<T> ExecuteAsync<T>(Func<TAggregate, Task<T>> func)
        {
            if (!this.State.IsInitialised)
                throw new GrainUninitialisedOnAccessException();

            var result = await func(this.State.Aggregate);

            this.RaiseEvents(this.eventRegistry.Events);
            await this.ConfirmEvents();
            await this.eventPublisher.PublishEvents(this.eventRegistry.Events);
            this.eventRegistry.Clear();

            return result;
        }
    }
}