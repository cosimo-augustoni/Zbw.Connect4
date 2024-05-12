using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application.Commands
{
    internal class UnreadyPlayerCommandHandler(IGameRepository gameRepository) : ICommandHandler<UnreadyPlayerCommand>
    {
        public async Task Handle(UnreadyPlayerCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.UnreadyPlayer(request.PlayerId);
        }
    }
}