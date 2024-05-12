using System.Diagnostics.CodeAnalysis;
using Game.Contract;

namespace Game.Domain.GameProjections
{
    public class GameView
    {
        public required GameId Id { get; init; }
        public required string Name { get; init; }

        [MemberNotNullWhen(true, nameof(CurrentPlayerId))]
        [MemberNotNullWhen(true, nameof(YellowPlayer))]
        [MemberNotNullWhen(true, nameof(RedPlayer))]
        public required bool IsRunning { get; init; }
        public required PlayerView? RedPlayer { get; init; }
        public required PlayerView? YellowPlayer { get; init; }
        public required PlayerId? CurrentPlayerId { get; init; }
        public required Board Board { get; init; }

        [MemberNotNullWhen(true, nameof(WinningPlayerId))]
        public required bool IsFinished { get; init; }
        public required PlayerId? WinningPlayerId { get; init; }
        public required bool IsAborted { get; init; }
    }
}