using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application.Commands
{
    internal class PlaceGamePieceCommandHandler(IGameRepository gameRepository) : ICommandHandler<PlaceGamePieceCommand>
    {
        public async Task Handle(PlaceGamePieceCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.PlaceGamePiece(request.BoardPosition);
        }
    }
}