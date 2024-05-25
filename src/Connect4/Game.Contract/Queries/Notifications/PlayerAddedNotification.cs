namespace Game.Contract.Queries.Notifications
{
    public record PlayerAddedNotification : GameNotification
    {
        public required PlayerId PlayerId { get; init; }
    }
}