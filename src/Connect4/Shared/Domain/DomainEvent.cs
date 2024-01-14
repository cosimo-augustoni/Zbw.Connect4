namespace Domain;

public abstract record class DomainEvent<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot<TAggregateRoot>;