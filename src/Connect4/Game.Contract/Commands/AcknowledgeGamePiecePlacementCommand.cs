using Shared.Contract;

namespace Game.Contract.Commands
{
    public record AcknowledgeGamePiecePlacementCommand(GameId GameId, PlayerId PlayerId) : ICommand;
}