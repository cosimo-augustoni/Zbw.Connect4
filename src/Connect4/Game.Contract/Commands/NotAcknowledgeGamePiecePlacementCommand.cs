using Shared.Contract;

namespace Game.Contract.Commands
{
    public record NotAcknowledgeGamePiecePlacementCommand(GameId GameId, PlayerId PlayerId) : ICommand;
}