using Shared.Contract;

namespace Game.Contract.Commands
{
    public record AddPlayerCommand(GameId GameId, Player Player, PlayerSide PlayerSide) : ICommand;
}