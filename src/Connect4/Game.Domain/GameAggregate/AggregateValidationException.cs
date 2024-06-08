using Game.Contract;
using Shared.Domain;

namespace Game.Domain.GameAggregate
{
    public class GamePiecePlacementAcknowledgementNotAllowedException : AggregateValidationException;

    public class SlotAlreadyFilledException : AggregateValidationException;

    public class GameNotStartedException : AggregateValidationException;

    public class GameAlreadyStartedException : AggregateValidationException;

    public class PlayerAlreadyInGameException : AggregateValidationException;

    public class PlayerNotReadyException : AggregateValidationException;

    public class PlayerSlotAlreadyOccupiedException(PlayerSide playerSide) : AggregateValidationException
    {
        public PlayerSide PlayerSide { get; } = playerSide;
    }
}