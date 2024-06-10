using Game.Contract;

namespace Game.Domain.GameAggregate
{
    public interface IGame
    {
        public Task<Guid> CreateGame(string? name);
        public Task ChangeNameAsync(string name);
        Task AddPlayer(Player player, PlayerSide playerSide);
        Task ReadyPlayer(PlayerId playerId);
        Task UnreadyPlayer(PlayerId playerId);
        Task PlaceGamePiece(BoardPosition boardPosition);
        Task AcknowledgeGamePiecePlacement(PlayerId playerId);
        Task NotAcknowledgeGamePiecePlacement(PlayerId playerId);
        Task AbortGame();

        Task<Board> GetBoardState();
        Task RemovePlayer(PlayerId playerId);
        Task StartGame();
    }
}
