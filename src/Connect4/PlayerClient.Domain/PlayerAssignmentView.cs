using Game.Contract;

namespace PlayerClient.Domain
{
    public class PlayerAssignmentView
    {
        public required PlayerId PlayerId { get; init; }

        public required GameId GameId { get; init; }
    }
}
