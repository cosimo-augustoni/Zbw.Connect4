using System.Diagnostics.CodeAnalysis;

namespace Game.Contract.Queries.Dtos
{
    public class GameDto
    {
        public required GameId Id { get; init; }
        public required string Name { get; init; }

        [MemberNotNullWhen(true, nameof(CurrentPlayerId))]
        [MemberNotNullWhen(true, nameof(YellowPlayer))]
        [MemberNotNullWhen(true, nameof(RedPlayer))]
        public required bool IsRunning { get; init; }
        public required PlayerDto? RedPlayer { get; init; }
        public required PlayerDto? YellowPlayer { get; init; }
        public required PlayerId? CurrentPlayerId { get; init; }
        public required BoardDto Board { get; init; }

        [MemberNotNullWhen(true, nameof(WinningPlayerId))]
        public required bool IsFinished { get; init; }
        public required PlayerId? WinningPlayerId { get; init; }
        public required bool IsAborted { get; init; }
    }
}