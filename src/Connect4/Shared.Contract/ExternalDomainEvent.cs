using MediatR;

namespace Shared.Contract
{
    public abstract record ExternalDomainEvent : INotification;
}