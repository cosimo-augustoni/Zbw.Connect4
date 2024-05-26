using Game.Contract;

namespace Game.Domain.GameProjections
{
    public class PlayerView
    {
        public required PlayerId Id { get; init; }
        public required PlayerOwner Owner { get; init; }
        public required bool IsReady { get; init; }
        public required string Type { get; init; }
    }
}
