using Shared.Contract;

namespace Shared.Infrastructure
{
    public interface IEventPublisher
    {
        Task PublishEvents(IReadOnlyList<DomainEvent> domainEvents);
    }
}