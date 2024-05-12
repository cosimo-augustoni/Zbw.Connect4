using MediatR;

namespace Game.Contract.Queries
{
    public record GameCreatedNotification : GameNotification;
    public record GameUpdatedNotification : GameNotification;

    public abstract record GameNotification : INotification
    {
        public required GameId GameId { get; init; }
    }

    public record PlayerAddedNotification : GameNotification;

    public record PlayerRemovedNotification : GameNotification;

    public record PlayerUpdatedNotification : GameNotification
    {
        public required PlayerId PlayerId { get; init; }
    }
}