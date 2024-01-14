namespace Domain
{
    public interface IAggregateRoot<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TAggregateRoot>
    {
        public IReadOnlyList<DomainEvent<TAggregateRoot>> Events { get; }
    }
}
