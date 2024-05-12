using Shared.Contract;

namespace Game.Contract.Commands
{
    public record AbortGameCommand(GameId GameId) : ICommand;
}