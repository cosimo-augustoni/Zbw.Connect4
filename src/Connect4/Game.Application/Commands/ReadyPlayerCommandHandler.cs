using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application.Commands
{
    internal class ReadyPlayerCommandHandler(IGameRepository gameRepository) : ICommandHandler<ReadyPlayerCommand>
    {
        public async Task Handle(ReadyPlayerCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.ReadyPlayer(request.PlayerId);
        }
    }
}