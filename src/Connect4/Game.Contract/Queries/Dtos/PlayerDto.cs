namespace Game.Contract.Queries.Dtos
{
    public class PlayerDto
    {
        public required PlayerId Id { get; init; }
        public required string Name { get; init; }
        public required bool IsReady { get; init; }
    }
}
