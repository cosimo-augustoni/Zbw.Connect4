using Shared.Contract;

namespace Game.Contract.Commands
{
    public record StartGameCommand(GameId GameId) : ICommand;
}