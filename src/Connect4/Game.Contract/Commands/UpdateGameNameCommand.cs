using Shared.Contract;

namespace Game.Contract.Commands
{
    public record UpdateGameNameCommand(GameId GameId, string Name) : ICommand;
}
