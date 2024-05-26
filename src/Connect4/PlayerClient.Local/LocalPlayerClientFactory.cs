using Game.Contract;
using MediatR;
using PlayerClient.Contract;
using PlayerClient.Domain;

namespace PlayerClient.Local
{
    internal class LocalPlayerClientFactory(ISender mediator, IPlayerAssignmentQuery playerAssignmentQuery) : IPlayerClientFactory
    {
        public PlayerClientType PlayerClientType => LocalPlayerClientConstants.PlayerClientType;

        public async Task<IPlayerClient?> CreateAsync(PlayerId playerId)
        {
            var gameId = await playerAssignmentQuery.GetGameIdByPlayerAsync(playerId);

            if (gameId == null)
                return null;

            return new LocalPlayerClient(mediator)
            {
                PlayerId = playerId,
                GameId = gameId
            };
        }
    }
}
