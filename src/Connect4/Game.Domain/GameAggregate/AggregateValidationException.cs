using Game.Contract;
using Game.Contract.Events;

namespace Game.Domain.GameAggregate
{
    public class AggregateValidationException : Exception
    {
    }

    public class GamePiecePlacementAcknowledgementNotAllowedException : AggregateValidationException
    {
    }

    public class SlotAlreadyFilledException : AggregateValidationException
    {
    }

    public class GameNotStartedException : AggregateValidationException
    {
    }

    public class GameAlreadyStartedException : AggregateValidationException
    {
    }

    public class PlayerAlreadyInGameException : AggregateValidationException
    {
    }

    public class PlayerSlotAlreadyOccupiedException(PlayerSide playerSide) : AggregateValidationException
    {
        public PlayerSide PlayerSide { get; } = playerSide;
    }
}