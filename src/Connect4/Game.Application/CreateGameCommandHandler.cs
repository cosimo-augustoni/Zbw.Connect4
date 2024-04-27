using Game.Domain;
using Shared.Application;

namespace Game.Application
{
    internal class CreateGameCommandHandler(IGameRepository gameRepository) : ICommandHandler<CreateGameCommand, Guid>
    {
        public async Task<Guid> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var game = gameRepository.Create();
            return await game.GetId();
        }
    }
}
