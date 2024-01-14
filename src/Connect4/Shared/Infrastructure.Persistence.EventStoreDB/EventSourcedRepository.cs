using Domain;
using EventStore.Client;
using Infrastructure.Persistence.EventStoreDB.EventRegistration;

namespace Infrastructure.Persistence.EventStoreDB
{
    public class EventSourcedRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TAggregateRoot>
    {
        private readonly EventStoreClient eventStoreClient;
        private readonly IAggregateRootFactory<TAggregateRoot> aggregateRootFactory;
        private readonly IEventTransformer<TAggregateRoot> eventTransformer;

        internal EventSourcedRepository(EventStoreClient eventStoreClient, IAggregateRootFactory<TAggregateRoot> aggregateRootFactory, IEventTransformer<TAggregateRoot> eventTransformer)
        {
            this.eventStoreClient = eventStoreClient;
            this.aggregateRootFactory = aggregateRootFactory;
            this.eventTransformer = eventTransformer;
        }

        public async Task<TAggregateRoot> Find<TKey>(TKey key, CancellationToken cancellationToken)
            where TKey : AggregateKey
        {
            var readStream = this.eventStoreClient.ReadStreamAsync(
                Direction.Forwards,
                typeof(TAggregateRoot).Name,
                StreamPosition.Start,
                cancellationToken: cancellationToken);
            var events = await readStream
                .Select(e => this.eventTransformer.Transform(e.Event.EventType, e.Event.Data.Span))
                .ToListAsync(cancellationToken);

            return this.aggregateRootFactory.Create(events);
        }

        public Task Add(TAggregateRoot aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Update(TAggregateRoot aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
