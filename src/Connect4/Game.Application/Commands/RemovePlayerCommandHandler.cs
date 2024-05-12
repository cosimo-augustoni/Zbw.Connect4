using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application.Commands
{
    internal class RemovePlayerCommandHandler(IGameRepository gameRepository) : ICommandHandler<RemovePlayerCommand>
    {
        public async Task Handle(RemovePlayerCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.RemovePlayer(request.PlayerId);
        }
    }
}