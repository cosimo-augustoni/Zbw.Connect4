using Shared.Contract;

namespace Game.Contract.Commands
{
    public record SurrenderCommand(GameId GameId, PlayerId PlayerId) : ICommand;
}