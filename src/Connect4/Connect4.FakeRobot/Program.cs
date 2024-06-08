using System.Collections.Concurrent;
using System.Timers;
using Connect4.ServiceDefaults;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Timer = System.Timers.Timer;

Console.WriteLine("Starting Fake Robot");
var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();
var host = builder.Build();
await host.RunAsync();

public class Robot
{
    private readonly IManagedMqttClient mqttClient;
    private readonly Timer timer;
    private string CurrentStatus { get; set; } = "0";
    private string Topic { get; }

    public Robot(IManagedMqttClient mqttClient, string robotId)
    {
        this.Topic = $"{robotId}_to_IT";
        this.mqttClient = mqttClient;

        this.timer = new Timer(TimeSpan.FromSeconds(6));
        this.timer.AutoReset = true;
        this.timer.Elapsed += this.TimerOnElapsed;
        this.timer.Start();
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        this.SendCurrentStatusAsync().GetAwaiter().GetResult();
    }

    public async Task MessageRecievedAsync(int message, CancellationToken cancellationToken)
    {
        this.Reset();

        var isSorting = (message & 128) == 128;
        if (isSorting)
        {
            Console.WriteLine($"{this.Topic} - Robot sorting");
            this.CurrentStatus = "4";
            await this.SendCurrentStatusAsync();

            await Task.Delay(5000, cancellationToken);

            Console.WriteLine($"{this.Topic} - Robot finished sorting");
            this.CurrentStatus = "5";
            await this.SendCurrentStatusAsync();
            return;
        }

        var xCoord = (message & 56) >> 3;
        var isYellowPlayer = (message & 64) == 64;
        var player = isYellowPlayer ? "yellow" : "red";
        Console.WriteLine($"{this.Topic} - Placing Piece for Player: {player} at X-Position: {xCoord}");
        this.CurrentStatus = "2";
        await this.SendCurrentStatusAsync();

        await Task.Delay((Math.Abs(xCoord - 7) * 1000) / 2, cancellationToken);

        Console.WriteLine($"{this.Topic} - Piece placed for Player: {player} at X-Position: {xCoord}");
        this.CurrentStatus = "3";
        await this.SendCurrentStatusAsync();

    }

    private async Task SendCurrentStatusAsync()
    {
        await this.mqttClient.EnqueueAsync(this.Topic, this.CurrentStatus);
    }

    public void Reset()
    {
        this.timer.Stop();
        this.timer.Start();
    }
}

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly ConcurrentDictionary<string, Robot> robots = new();
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly IManagedMqttClient mqttClient = new MqttFactory().CreateManagedMqttClient();

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var mqttHost = Environment.GetEnvironmentVariable("MQTT_HOST");
        var mqttUser = Environment.GetEnvironmentVariable("MQTT_USERNAME");
        var mqttPassword = Environment.GetEnvironmentVariable("MQTT_PASSWORD");

        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(mqttHost)
            .WithCredentials(mqttUser, mqttPassword)
            .Build();

        var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
            .WithClientOptions(mqttClientOptions)
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(3))
            .Build();

        this.mqttClient.ConnectingFailedAsync += _ =>
        {
            logger.LogWarning("Couldn't connect to MQTT. Retry in 3 seconds");
            return Task.CompletedTask;
        };
        this.mqttClient.ConnectedAsync += _ =>
        {
            logger.LogInformation("Connection to MQTT established.");
            return Task.CompletedTask;
        };

        logger.LogInformation("Starting MQTT Client.");
        await this.mqttClient.StartAsync(managedMqttClientOptions);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.mqttClient.ApplicationMessageReceivedAsync += eventArgs =>
        {
            if (!eventArgs.ApplicationMessage.Topic.StartsWith("IT_to_Fake"))
                return Task.CompletedTask;

            try
            {
                var payload = eventArgs.ApplicationMessage.ConvertPayloadToString();
                var topic = eventArgs.ApplicationMessage.Topic;
                logger.LogInformation($"Message recieved: Topic: {topic} Payload: {payload}");

                var robotId = topic.Split('_')[2];
                if (!int.TryParse(payload, out var message))
                    logger.LogError($"Couldn't process message: {payload}");

                if (!this.robots.TryGetValue(robotId, out var robot))
                {
                    robot = new Robot(this.mqttClient, robotId);
                    this.robots.TryAdd(robotId, robot);
                }

                _ = Task.Run(async () =>
                {
                    await robot.MessageRecievedAsync(message, this.cancellationTokenSource.Token);
                }, this.cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Couldn't process message");
                throw;
            }

            return Task.CompletedTask;
        };

        logger.LogInformation("Subscribing to Topic: #");
        await this.mqttClient.SubscribeAsync("#");

        var fakeCount = Environment.GetEnvironmentVariable("NUMBER_OF_INITIAL_FAKES");
        if (fakeCount != null && int.TryParse(fakeCount, out var fakesToCreate))
        {
            for (var i = 1; i <= fakesToCreate; i++)
            {
                var robotId = $"Fake{i:D3}";
                this.robots.TryAdd(robotId, new Robot(this.mqttClient, robotId));
            }
        }

        logger.LogInformation("Fake Robot started.");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Fake Robot.");
        await this.mqttClient.StopAsync();
        await this.cancellationTokenSource.CancelAsync();
        await base.StopAsync(cancellationToken);
    }
}