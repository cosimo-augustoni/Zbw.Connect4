namespace Domain;

public interface IRepository<TAggregateRoot> : IReadOnlyRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    Task Add(TAggregateRoot aggregate, CancellationToken cancellationToken);
    Task Update(TAggregateRoot aggregate, CancellationToken cancellationToken);
}