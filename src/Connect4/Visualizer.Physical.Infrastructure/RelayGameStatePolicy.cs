using Game.Contract;
using Game.Contract.Events;
using MediatR;
using Visualizer.Contract;
using Visualizer.Contract.Commands;
using Visualizer.Contract.Events;
using Visualizer.Contract.Queries;

namespace Visualizer.Physical.Infrastructure
{
    internal class RelayGameStatePolicy(IVisualizerMqttClient mqttClient, IMediator mediator)
        : INotificationHandler<GamePiecePlacedEventDto>
            , INotificationHandler<GameFinishedEventDto>
            , INotificationHandler<GameAbortedEventDto>
            , INotificationHandler<VisualizerAddedToGameEvent>
    {
        public async Task Handle(GamePiecePlacedEventDto notification, CancellationToken cancellationToken)
        {
            var visualizer = await mediator.Send(new VisualizerByGameIdQuery { GameId = notification.GameId }, cancellationToken);
            if (visualizer == null)
                return;

            var playerSide = notification.PlayingSide == PlayerSide.Red ? MqttMessages.PlaceRed : MqttMessages.PlaceYellow;
            var xCoord = notification.Position.X << 3;
            var yCoord = notification.Position.Y;
            var payload = playerSide + xCoord + yCoord;

            await mediator.Send(new ChangeVisualizerStatusCommand{VisualizerId = visualizer.Id, Status = VisualizerStatus.Processing}, cancellationToken);
            await mqttClient.PublishAsync(visualizer.GetMqttTopic(), payload.ToString());
        }

        public async Task Handle(GameFinishedEventDto notification, CancellationToken cancellationToken)
        {
            var visualizer = await mediator.Send(new VisualizerByGameIdQuery { GameId = notification.GameId }, cancellationToken);
            if (visualizer == null)
                return;

            await mqttClient.PublishAsync(visualizer.GetMqttTopic(), MqttMessages.Sort.ToString());
        }

        public async Task Handle(GameAbortedEventDto notification, CancellationToken cancellationToken)
        {
            var visualizer = await mediator.Send(new VisualizerByGameIdQuery { GameId = notification.GameId }, cancellationToken);
            if (visualizer == null || visualizer.Status == VisualizerStatus.PiecesSorted || visualizer.Status == VisualizerStatus.SortingPieces)
                return;

            await mqttClient.PublishAsync(visualizer.GetMqttTopic(), MqttMessages.Sort.ToString("D"));
        }

        public async Task Handle(VisualizerAddedToGameEvent notification, CancellationToken cancellationToken)
        {
            var visualizer = await mediator.Send(new VisualizerByGameIdQuery { GameId = notification.GameId }, cancellationToken);
            if (visualizer == null || visualizer.Status == VisualizerStatus.PiecesSorted || visualizer.Status == VisualizerStatus.SortingPieces)
                return;

            await mqttClient.PublishAsync(visualizer.GetMqttTopic(), MqttMessages.Sort.ToString("D"));
        }

        private static class MqttMessages
        {
            public const int PlaceRed = 0;
            public const int PlaceYellow = 64;
            public const int Sort = 128;
        }
    }

    file static class VisualizerExtensions
    {
        public static string GetMqttTopic(this VisualizerDto visualizer)
        {
            return $"IT_to_{visualizer.ExternalId}";
        }
    }
}
