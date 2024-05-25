namespace Game.Contract.Queries.Notifications
{
    public record PlayerRemovedNotification : GameNotification
    {
        public required PlayerId PlayerId { get; init; }
    }
}