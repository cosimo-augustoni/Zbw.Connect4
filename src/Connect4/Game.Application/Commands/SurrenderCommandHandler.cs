using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application.Commands
{
    internal class SurrenderCommandHandler(IGameRepository gameRepository) : ICommandHandler<SurrenderCommand>
    {
        public async Task Handle(SurrenderCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.Surrender(request.PlayerId);
        }
    }
}