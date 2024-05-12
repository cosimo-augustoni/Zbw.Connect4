using Shared.Contract;

namespace Game.Contract.Commands
{
    public record RemovePlayerCommand(GameId GameId, PlayerId PlayerId) : ICommand;
}