using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Visualizer.Contract.Queries;

namespace Visualizer.Physical.Infrastructure
{
    public class VisualizerMqttClient(IConfiguration configuration, ILogger<VisualizerMqttClient> logger) : IVisualizerMqttClient
    {
        private readonly IManagedMqttClient mqttClient = new MqttFactory().CreateManagedMqttClient();

        public event Func<MqttApplicationMessageReceivedEventArgs, Task> MessageReceivedAsync
        {
            add { this.mqttClient.ApplicationMessageReceivedAsync += value; }
            remove { this.mqttClient.ApplicationMessageReceivedAsync -= value; }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var mqttHost = configuration.GetValue<string>("MQTT_HOST");
            if (string.IsNullOrWhiteSpace(mqttHost))
                throw new ArgumentNullException();
            var mqttUser = configuration.GetValue<string>("MQTT_USERNAME");
            if (string.IsNullOrWhiteSpace(mqttHost))
                throw new ArgumentNullException();
            var mqttPassword = configuration.GetValue<string>("MQTT_PASSWORD");
            if (string.IsNullOrWhiteSpace(mqttHost))
                throw new ArgumentNullException();

            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttHost)
                .WithCredentials(mqttUser, mqttPassword)
                .Build();

            var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(mqttClientOptions)
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(3))
                .Build();

            this.mqttClient.ConnectingFailedAsync += this.MqttClientOnConnectingFailedAsync;
            this.mqttClient.ConnectedAsync += this.MqttClientOnConnectedAsync;
            await this.mqttClient.StartAsync(managedMqttClientOptions);
        }

        private Task MqttClientOnConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            logger.LogInformation($"MQTT Connection Established: {arg.ConnectResult?.ResultCode}");
            return Task.CompletedTask;
        }

        private Task MqttClientOnConnectingFailedAsync(ConnectingFailedEventArgs arg)
        {
            logger.LogWarning(arg.Exception, $"MQTT Connection Failed");
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.mqttClient.ConnectingFailedAsync -= this.MqttClientOnConnectingFailedAsync;
            this.mqttClient.ConnectedAsync -= this.MqttClientOnConnectedAsync;
            await this.mqttClient.StopAsync();
            this.mqttClient.Dispose();
        }

        public async Task SubscribeAsync(string topic)
        {
            await this.mqttClient.SubscribeAsync(topic);
        }

        public async Task UnsubscribeAsync(string topic)
        {
            await this.mqttClient.UnsubscribeAsync(topic);
        }

        public async Task SubscribeWithExternalIdAsync(string externalId)
        {
            await this.SubscribeAsync($"{externalId}_to_IT");
        }

        public async Task UnsubscribeWithExternalIdAsync(string externalId)
        {
            await this.UnsubscribeAsync($"{externalId}_to_IT");
        }

        public async Task PublishAsync(string topic, string payload)
        {
            await this.mqttClient.EnqueueAsync(topic, payload);
            SpinWait.SpinUntil(() => this.mqttClient.PendingApplicationMessagesCount == 0, 3000);
        }
    }
}