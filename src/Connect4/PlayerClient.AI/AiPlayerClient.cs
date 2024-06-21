using Game.Contract;
using Game.Contract.Commands;
using MediatR;
using PlayerClient.Contract;

namespace PlayerClient.AI
{
    public class AiPlayerClient(ISender mediator) : IPlayerClient
    {
        public required PlayerId PlayerId { get; init; }
        public required GameId GameId { get; init; }
        public async Task Ready()
        {
            await mediator.Send(new ReadyPlayerCommand(this.GameId, this.PlayerId));
        }

        public Task Unready()
        {
            throw new NotSupportedException();
        }

        public async Task Leave()
        {
            await mediator.Send(new RemovePlayerCommand(this.GameId, this.PlayerId));
        }

        public Task Surrender()
        {
            throw new NotSupportedException();
        }

        public Task RequestGamePiecePlacementAcknowledgement(BoardPosition notificationPosition)
        {
            _ = Task.Run(async () => await mediator.Send(new AcknowledgeGamePiecePlacementCommand(this.GameId, this.PlayerId)));
            return Task.CompletedTask;
        }
    }
}
