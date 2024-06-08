using Game.Contract.Queries.Dtos;

namespace Game.Contract
{
    public static class PlayerSideExtensions
    {
        public static SlotState ToSlotState(this PlayerSide playerSide) => playerSide switch
        {
            PlayerSide.Red => SlotState.Red,
            PlayerSide.Yellow => SlotState.Yellow,
            _ => throw new ArgumentOutOfRangeException(nameof(playerSide), playerSide, null)
        };
    }
}