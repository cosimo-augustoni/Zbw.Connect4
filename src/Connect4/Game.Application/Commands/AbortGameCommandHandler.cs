using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application.Commands
{
    internal class AbortGameCommandHandler(IGameRepository gameRepository) : ICommandHandler<AbortGameCommand>
    {
        public async Task Handle(AbortGameCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.AbortGame();
        }
    }
}