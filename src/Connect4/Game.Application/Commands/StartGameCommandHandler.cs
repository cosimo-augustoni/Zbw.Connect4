using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application.Commands
{
    internal class StartGameCommandHandler(IGameRepository gameRepository) : ICommandHandler<StartGameCommand>
    {
        public async Task Handle(StartGameCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.StartGame();
        }
    }
}