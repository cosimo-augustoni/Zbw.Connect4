namespace Domain;

public abstract class AggregateRoot<TAggregateRoot> : Entity<TAggregateRoot>, IAggregateRoot<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot<TAggregateRoot>
{
    private readonly List<DomainEvent<TAggregateRoot>> events = new();

    public IReadOnlyList<DomainEvent<TAggregateRoot>> Events => this.events.AsReadOnly();

    public abstract AggregateKey Id { get; }

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