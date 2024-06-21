using Game.Contract;
using Game.Contract.Commands;
using MediatR;
using PlayerClient.Contract;

namespace PlayerClient.Local
{
    public class LocalPlayerClient(ISender mediator) : IPlayerClient
    {
        public required PlayerId PlayerId { get; set; }
        public required GameId GameId { get; set; }

        public async Task Ready()
        {
            await mediator.Send(new ReadyPlayerCommand(this.GameId, this.PlayerId));
        }

        public async Task Unready()
        {
            await mediator.Send(new UnreadyPlayerCommand(this.GameId, this.PlayerId));
        }

        public async Task Leave()
        {
            await mediator.Send(new RemovePlayerCommand(this.GameId, this.PlayerId));
        }

        public async Task Surrender()
        {
            await mediator.Send(new SurrenderCommand(this.GameId, this.PlayerId));
        }

        public Task RequestGamePiecePlacementAcknowledgement(BoardPosition notificationPosition)
        {
            _ = Task.Run(async () => await mediator.Send(new AcknowledgeGamePiecePlacementCommand(this.GameId, this.PlayerId)));
            return Task.CompletedTask;
        }
    }
}
