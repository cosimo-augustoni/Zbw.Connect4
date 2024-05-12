using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application.Commands
{
    internal class AddPlayerCommandHandler(IGameRepository gameRepository) : ICommandHandler<AddPlayerCommand>
    {
        public async Task Handle(AddPlayerCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.AddPlayer(request.Player, request.PlayerSide);
        }
    }
}