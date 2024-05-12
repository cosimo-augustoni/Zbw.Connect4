using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application.Commands
{
    internal class AcknowledgeGamePiecePlacementCommandHandler(IGameRepository gameRepository) : ICommandHandler<AcknowledgeGamePiecePlacementCommand>
    {
        public async Task Handle(AcknowledgeGamePiecePlacementCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.AcknowledgeGamePiecePlacement(request.PlayerId);
        }
    }
}