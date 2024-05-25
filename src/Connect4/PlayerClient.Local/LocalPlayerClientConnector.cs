using Game.Contract.Commands;
using MediatR;
using Game.Contract;
using PlayerClient.Contract;
using GameId = PlayerClient.Contract.GameId;

namespace PlayerClient.Local
{
    internal class LocalPlayerClientConnector(ISender mediator) : IPlayerClientConnector
    {
        public async Task JoinGame(GameId gameId, User user, PlayerSide playerSide)
        {
            var playerId = new Game.Contract.PlayerId();
            var player = new Player(playerId, user.Identifier, LocalPlayerClientConstants.PlayerClientType.Identifier);
            await mediator.Send(new AddPlayerCommand(gameId.ToGameId(), player, playerSide));
        }
    }
}
