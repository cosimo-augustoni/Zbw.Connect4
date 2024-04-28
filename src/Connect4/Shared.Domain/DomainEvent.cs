using MediatR;

namespace Shared.Domain
{
    public abstract record DomainEvent : INotification;
}
