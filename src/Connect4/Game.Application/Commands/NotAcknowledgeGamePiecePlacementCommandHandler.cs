using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application.Commands
{
    internal class NotAcknowledgeGamePiecePlacementCommandHandler(IGameRepository gameRepository) : ICommandHandler<NotAcknowledgeGamePiecePlacementCommand>
    {
        public async Task Handle(NotAcknowledgeGamePiecePlacementCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.NotAcknowledgeGamePiecePlacement(request.PlayerId);
        }
    }
}