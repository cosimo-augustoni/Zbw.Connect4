using Domain;

namespace Infrastructure.Persistence.EventStoreDB
{
    public interface IAggregateRootFactory<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TAggregateRoot>
    {
        TAggregateRoot Create(IEnumerable<DomainEvent<TAggregateRoot>> events);
    }
}
