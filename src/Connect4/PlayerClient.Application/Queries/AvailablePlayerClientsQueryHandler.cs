using Game.Contract;
using PlayerClient.Contract;
using PlayerClient.Contract.Queries;
using Shared.Application;

namespace PlayerClient.Application.Queries
{
    internal class AvailablePlayerClientsQueryHandler(IEnumerable<IPlayerClientConnector> clientConnectors) : IQueryHandler<AvailablePlayerClientsQuery, IEnumerable<IPlayerClientConnector>>
    {
        public Task<IEnumerable<IPlayerClientConnector>> Handle(AvailablePlayerClientsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(clientConnectors);
        }
    }
}
