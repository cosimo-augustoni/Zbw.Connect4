using Game.Contract.Commands;
using Game.Domain.GameAggregate;
using Shared.Application;

namespace Game.Application
{
    internal class UpdateGameNameCommandHandler(IGameRepository gameRepository) : ICommandHandler<UpdateGameNameCommand>
    {
        public async Task Handle(UpdateGameNameCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.ChangeNameAsync(request.Name);
        }
    }
}
