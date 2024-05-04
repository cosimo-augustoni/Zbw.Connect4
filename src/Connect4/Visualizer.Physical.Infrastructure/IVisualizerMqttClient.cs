using MQTTnet.Client;
using Visualizer.Contract.Queries;

namespace Visualizer.Physical.Infrastructure
{
    public interface IVisualizerMqttClient
    {
        event Func<MqttApplicationMessageReceivedEventArgs, Task> MessageReceivedAsync;
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        Task SubscribeAsync(string topic);
        Task SubscribeWithExternalIdAsync(string externalId);
        Task PublishAsync(string topic, string payload);
        Task UnsubscribeAsync(string topic);
        Task UnsubscribeWithExternalIdAsync(string externalId);
    }
}