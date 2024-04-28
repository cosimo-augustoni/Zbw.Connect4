using Shared.Domain;

namespace Shared.Infrastructure
{
    public interface IEventPublisher
    {
        Task PublishEvents(IReadOnlyList<DomainEvent> domainEvents);
    }
}