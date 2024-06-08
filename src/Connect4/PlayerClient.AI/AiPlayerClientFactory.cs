using Game.Contract;
using MediatR;
using PlayerClient.Contract;
using PlayerClient.Domain;

namespace PlayerClient.AI
{
    internal class AiPlayerClientFactory(ISender mediator, IPlayerAssignmentQuery playerAssignmentQuery) : IPlayerClientFactory
    {
        public List<PlayerClientType> PlayerClientTypes =>
        [
            AiPlayerClientConstants.EasyPlayerClientType,
            AiPlayerClientConstants.MediumPlayerClientType,
            AiPlayerClientConstants.HardPlayerClientType
        ];

        public async Task<IPlayerClient?> CreateAsync(PlayerId playerId)
        {
            var gameId = await playerAssignmentQuery.GetGameIdByPlayerAsync(playerId);

            if (gameId == null)
                return null;

            return new AiPlayerClient(mediator)
            {
                PlayerId = playerId,
                GameId = gameId
            };
        }
    }
}
