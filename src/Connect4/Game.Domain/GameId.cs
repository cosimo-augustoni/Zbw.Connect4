namespace Game.Domain
{
    public record GameId(Guid Id)
    {
        public GameId() : this(Guid.NewGuid())
        {
        }
    }
}