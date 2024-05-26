namespace Game.Contract.Queries.Dtos
{
    public class PlayerDto
    {
        public required PlayerId Id { get; init; }
        public required PlayerOwnerDto Owner { get; init; }
        public required bool IsReady { get; init; }
        public required string Type { get; init; }
    }
}
