using Game.Domain;
using Shared.Application;

namespace Game.Application
{
    public record UpdateGameNameCommand(GameId GameId, string Name) : ICommand;
}
