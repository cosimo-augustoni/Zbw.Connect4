namespace Shared.Contract
{
    public interface IExternalDomainEvent<out TExternalDomainEvent> : IExternalDomainEvent
        where TExternalDomainEvent : ExternalDomainEvent
    {
        new TExternalDomainEvent ToExternalDomainEvent();

        ExternalDomainEvent IExternalDomainEvent.ToExternalDomainEvent()
        {
            return this.ToExternalDomainEvent();
        }
    }

    public interface IExternalDomainEvent
    {
        ExternalDomainEvent ToExternalDomainEvent();
    }
}