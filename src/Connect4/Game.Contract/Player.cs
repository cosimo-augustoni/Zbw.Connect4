namespace Game.Contract
{
    public record Player(PlayerId Id, PlayerOwner PlayerOwner, string TypeIdentifier, bool IsReady = false);
}