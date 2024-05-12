using Shared.Contract;

namespace Game.Contract.Commands
{
    public record PlaceGamePieceCommand(GameId GameId, BoardPosition BoardPosition) : ICommand;
}