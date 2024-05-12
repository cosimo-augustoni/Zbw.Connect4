using Game.Contract;

namespace Game.Domain.GameProjections
{
    public class PlayerView
    {
        public required PlayerId Id { get; init; }
        public required string Name { get; init; }
        public required bool IsReady { get; init; }
    }
}
