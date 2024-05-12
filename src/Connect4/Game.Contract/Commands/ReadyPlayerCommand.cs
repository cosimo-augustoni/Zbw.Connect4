using Shared.Contract;

namespace Game.Contract.Commands
{
    public record ReadyPlayerCommand(GameId GameId, PlayerId PlayerId) : ICommand;
}