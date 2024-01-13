namespace Domain;

public interface IEventAware<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    void Apply(DomainEvent<TAggregateRoot> @event);
}