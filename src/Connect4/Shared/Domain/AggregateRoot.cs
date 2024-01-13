namespace Domain;

public abstract class AggregateRoot<TAggregateRoot> : Entity<TAggregateRoot>, IAggregateRoot
    where TAggregateRoot : IAggregateRoot
{
    private readonly List<DomainEvent<TAggregateRoot>> events = new();

    protected void RegisterEvent(DomainEvent<TAggregateRoot> @event)
    {
        this.events.Add(@event);
    }

    protected void ApplyAndRegisterEvent(DomainEvent<TAggregateRoot> @event)
    {
        this.Apply(@event);
        this.RegisterEvent(@event);
    }
}