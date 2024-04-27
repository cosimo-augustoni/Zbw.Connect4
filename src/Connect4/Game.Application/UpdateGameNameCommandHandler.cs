using Game.Domain;
using Shared.Application;

namespace Game.Application
{
    internal class UpdateGameNameCommandHandler(IGameRepository gameRepository) : ICommandHandler<UpdateGameNameCommand>
    {
        public async Task Handle(UpdateGameNameCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.GetById(request.GameId);
            await game.UpdateGameNameAsync(request.Name);
        }
    }
}
