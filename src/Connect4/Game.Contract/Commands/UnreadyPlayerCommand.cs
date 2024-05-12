using Shared.Contract;

namespace Game.Contract.Commands
{
    public record UnreadyPlayerCommand(GameId GameId, PlayerId PlayerId) : ICommand;
}