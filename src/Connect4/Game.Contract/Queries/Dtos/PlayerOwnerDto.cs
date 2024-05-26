namespace Game.Contract.Queries.Dtos
{
    public class PlayerOwnerDto
    {
        public required string Identifier { get; init; }
        public required string DisplayName { get; init; }
    }
}