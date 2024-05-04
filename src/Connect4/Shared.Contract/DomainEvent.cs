using MediatR;

namespace Shared.Contract
{
    public abstract record DomainEvent : INotification;
}
