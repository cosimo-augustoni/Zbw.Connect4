namespace Domain;

public interface IEventAware<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot<TAggregateRoot>
{
    void Apply(DomainEvent<TAggregateRoot> @event);
}