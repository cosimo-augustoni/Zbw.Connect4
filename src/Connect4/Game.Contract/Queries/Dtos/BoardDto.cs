namespace Game.Contract.Queries.Dtos
{
    public class BoardDto
    {
        public required SlotDto[][] BoardState { get; init; }
    }
}