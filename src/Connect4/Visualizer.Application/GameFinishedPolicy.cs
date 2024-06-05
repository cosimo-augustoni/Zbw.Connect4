using Game.Contract;
using Game.Contract.Events;
using MediatR;
using Visualizer.Contract.Commands;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application
{
    internal class GameFinishedPolicy(IMediator mediator, IVisualizerQuery visualizerQuery) 
        : INotificationHandler<GameFinishedEvent>
        , INotificationHandler<GameAbortedEvent>
    {
        public async Task Handle(GameFinishedEvent notification, CancellationToken cancellationToken)
        {
            await this.RemoveVisualizerFromGameAsync(notification.GameId, cancellationToken);
        }

        public async Task Handle(GameAbortedEvent notification, CancellationToken cancellationToken)
        {
            await this.RemoveVisualizerFromGameAsync(notification.GameId, cancellationToken);
        }

        private async Task RemoveVisualizerFromGameAsync(GameId gameId, CancellationToken cancellationToken)
        {
            var visualizer = await visualizerQuery.GetByGameIdAsync(gameId, cancellationToken);
            if (visualizer == null)
                return;

            await mediator.Send(new RemoveVisualizerFromGameCommand { VisualizerId = visualizer.Id }, cancellationToken);
        }
    }
}
