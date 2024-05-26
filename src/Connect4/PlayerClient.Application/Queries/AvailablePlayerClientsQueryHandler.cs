using Game.Contract;
using PlayerClient.Contract;
using PlayerClient.Contract.Queries;
using Shared.Application;

namespace PlayerClient.Application.Queries
{
    internal class AvailablePlayerClientsQueryHandler(IEnumerable<IPlayerClientConnector> clientConnectors) : IQueryHandler<AvailablePlayerClientsQuery, IReadOnlyList<IPlayerClientConnector>>
    {
        public Task<IReadOnlyList<IPlayerClientConnector>> Handle(AvailablePlayerClientsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyList<IPlayerClientConnector>>(clientConnectors.ToList());
        }
    }
}
