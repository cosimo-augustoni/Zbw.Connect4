using Game.Contract;
using Game.Contract.Commands;
using MediatR;
using PlayerClient.Contract;
using PlayerClient.Domain;
using GameId = PlayerClient.Contract.GameId;
using PlayerId = PlayerClient.Contract.PlayerId;

namespace PlayerClient.Local
{
    public class LocalPlayerClient(ISender mediator) : IPlayerClient
    {
        public required PlayerId PlayerId { get; set; }
        public required GameId GameId { get; set; }

        public async Task Ready()
        {
            await mediator.Send(new ReadyPlayerCommand(this.GameId.ToGameId(), this.PlayerId.ToPlayerId()));
        }

        public async Task Unready()
        {
            await mediator.Send(new UnreadyPlayerCommand(this.GameId.ToGameId(), this.PlayerId.ToPlayerId()));
        }

        public async Task Leave()
        {
            await mediator.Send(new RemovePlayerCommand(this.GameId.ToGameId(), this.PlayerId.ToPlayerId()));
        }
    }

    internal static class PlayerClientIdExtensions
    {
        public static Game.Contract.GameId ToGameId(this GameId gameId) => new(gameId.Id);
        public static Game.Contract.PlayerId ToPlayerId(this PlayerId gameId) => new(gameId.Id);
    }
}
