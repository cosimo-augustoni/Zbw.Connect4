using Game.Contract;
using Shared.Contract;

namespace PlayerClient.Contract.Queries
{
    public class PlayerClientByPlayerQuery : IQuery<IPlayerClient?>
    {
        public required PlayerId PlayerId { get; init; }
        public required PlayerClientType PlayerClientType { get; init; }
    }
}