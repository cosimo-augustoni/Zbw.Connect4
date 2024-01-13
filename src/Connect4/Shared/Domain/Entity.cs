namespace Domain
{
    public abstract class Entity<TAggregateRoot> : IEventAware<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        protected abstract void Apply(DomainEvent<TAggregateRoot> @event);

        void IEventAware<TAggregateRoot>.Apply(DomainEvent<TAggregateRoot> @event)
        {
            this.Apply(@event);
        }
    }
}
