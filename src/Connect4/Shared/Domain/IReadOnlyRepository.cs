namespace Domain;

public interface IReadOnlyRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    Task<TAggregateRoot> Find<TKey>(TKey key, CancellationToken cancellationToken)
        where TKey : AggregateKey;
}