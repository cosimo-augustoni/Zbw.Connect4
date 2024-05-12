namespace Game.Contract
{
    public record Player(PlayerId Id, string Name, string TypeIdentifier, bool IsReady = false)
    {
        public PlayerId Id { get; init; } = Id;
        public string Name { get; init; } = Name;
        public string TypeIdentifier { get; init; } = TypeIdentifier;
        public bool IsReady { get; init; } = IsReady;
    }
}