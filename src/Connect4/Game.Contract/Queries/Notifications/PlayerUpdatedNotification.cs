namespace Game.Contract.Queries.Notifications
{
    public record PlayerUpdatedNotification : GameNotification
    {
        public required PlayerId PlayerId { get; init; }
    }
}