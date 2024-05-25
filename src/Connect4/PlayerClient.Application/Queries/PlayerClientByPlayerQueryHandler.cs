using PlayerClient.Contract;
using PlayerClient.Contract.Queries;
using PlayerClient.Domain;
using Shared.Application;

namespace PlayerClient.Application.Queries
{
    internal class PlayerClientByPlayerQueryHandler(IEnumerable<IPlayerClientFactory> clientFactories) : IQueryHandler<PlayerClientByPlayerQuery, IPlayerClient>
    {
        public async Task<IPlayerClient> Handle(PlayerClientByPlayerQuery request, CancellationToken cancellationToken)
        {
            foreach (var clientFactory in clientFactories)
            {
                if (clientFactory.PlayerClientType == request.PlayerClientType)
                    return await clientFactory.CreateAsync(request.PlayerId);
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}