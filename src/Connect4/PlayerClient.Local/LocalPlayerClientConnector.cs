using Game.Contract.Commands;
using MediatR;
using Game.Contract;
using PlayerClient.Contract;

namespace PlayerClient.Local
{
    internal class LocalPlayerClientConnector(ISender mediator) : IPlayerClientConnector
    {
        public string DisplayName => "Beitreten";

        public async Task JoinGame(GameId gameId, User user, PlayerSide playerSide)
        {
            var playerId = new PlayerId();
            var player = new Player(playerId, new PlayerOwner(user.Identifier, user.DisplayName), LocalPlayerClientConstants.PlayerClientType.Identifier);
            await mediator.Send(new AddPlayerCommand(gameId, player, playerSide));
        }
    }
}
