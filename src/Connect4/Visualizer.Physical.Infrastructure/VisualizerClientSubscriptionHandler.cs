using MediatR;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using Visualizer.Contract;
using Visualizer.Contract.Commands;
using Visualizer.Contract.Queries;

namespace Visualizer.Physical.Infrastructure
{
    internal class VisualizerClientSubscriptionHandler(ISender mediator, IVisualizerMqttClient mqttClient, IVisualizerStatusWatcher visualizerStatusWatcher) : BackgroundService
    {
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await mqttClient.StartAsync(cancellationToken);
            await visualizerStatusWatcher.StartAsync();
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            mqttClient.MessageReceivedAsync += this.OnMessageReceivedAsync;
            var visualizers = await mediator.Send(new AllVisualizersQuery(), stoppingToken);
            foreach (var visualizer in visualizers.Where(v => !string.IsNullOrWhiteSpace(v.ExternalId)))
            {
                await mqttClient.SubscribeWithExternalIdAsync(visualizer.ExternalId!);
            }
        }

        private async Task OnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            var payload = arg.ApplicationMessage.ConvertPayloadToString();

            if (!int.TryParse(payload, out var statusCode))
                throw new VisualizerStatusCodeNotValidException();

            var visualizerExternalId = arg.ApplicationMessage.Topic.Split('_')[0];
            var visualizer = await mediator.Send(new VisualizerByExternalIdQuery
            {
                ExternalId = visualizerExternalId
            });

            visualizerStatusWatcher.StatusUpdateReceived(visualizer.Id);
            await mediator.Send(new ChangeVisualizerStatusCommand
            {
                VisualizerId = visualizer.Id,
                Status = VisualizerStatus.GetById(statusCode)
            });
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            mqttClient.MessageReceivedAsync -= this.OnMessageReceivedAsync;
            await visualizerStatusWatcher.StopAsync();
            await mqttClient.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}