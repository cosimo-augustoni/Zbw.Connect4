using Shared.Contract;

namespace PlayerClient.Contract.Queries
{
    public class AvailablePlayerClientsQuery : IQuery<IEnumerable<IPlayerClientConnector>>;
}
