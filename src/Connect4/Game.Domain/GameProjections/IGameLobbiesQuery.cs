namespace Game.Domain.GameProjections
{
    public interface IGameLobbiesQuery
    {
        Task<IReadOnlyList<GameLobby>> GetAllLobbies();
    }
}
