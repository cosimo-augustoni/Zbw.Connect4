using MediatR;

namespace Game.Contract.Queries.Notifications
{
    public abstract record GameNotification : INotification
    {
        public required GameId GameId { get; init; }
    }
}