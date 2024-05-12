using Game.Contract;

namespace Game.Infrastructure.GameProjections.Players
{
    internal class PlayerViewDbo
    {
        public Guid PlayerId { get; set; }
        public required string Name { get; set; }
        public bool IsReady { get; set; }
    }
}
