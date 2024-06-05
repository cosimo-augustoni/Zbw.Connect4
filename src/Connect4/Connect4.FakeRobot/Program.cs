using System.Collections.Concurrent;
using System.Timers;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Timer = System.Timers.Timer;

Console.WriteLine("Starting Fake Robot");
var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;

await Task.Run((Func<Task>)(async () =>
{
    var robots = new ConcurrentDictionary<string, Robot>();

    var mqttHost = Environment.GetEnvironmentVariable("MQTT_HOST");
    var mqttUser = Environment.GetEnvironmentVariable("MQTT_USERNAME");
    var mqttPassword = Environment.GetEnvironmentVariable("MQTT_PASSWORD");

    var mqttClient = new MqttFactory().CreateManagedMqttClient();
    var mqttClientOptions = new MqttClientOptionsBuilder()
        .WithTcpServer(mqttHost)
        .WithCredentials(mqttUser, mqttPassword)
        .Build();

    var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
        .WithClientOptions(mqttClientOptions)
        .WithAutoReconnectDelay(TimeSpan.FromSeconds(3))
        .Build();

    mqttClient.ConnectingFailedAsync += _ =>
    {
        WriteError("Couldn't connect to MQTT. Retry in 3 seconds");
        return Task.CompletedTask;
    };
    mqttClient.ConnectedAsync += _ =>
    {
        Console.WriteLine("Connection to MQTT established.");
        return Task.CompletedTask;
    };

    Console.WriteLine("Starting MQTT Client.");
    await mqttClient.StartAsync(managedMqttClientOptions);

    mqttClient.ApplicationMessageReceivedAsync += eventArgs =>
    {
        if (!eventArgs.ApplicationMessage.Topic.StartsWith("IT_to_Fake"))
            return Task.CompletedTask;

        try
        {
            var payload = eventArgs.ApplicationMessage.ConvertPayloadToString();
            var topic = eventArgs.ApplicationMessage.Topic;
            Console.WriteLine($"Message recieved: Topic: {topic} Payload: {payload}");

            var robotId = topic.Split('_')[2];
            if (!int.TryParse(payload, out var message))
                WriteError($"Couldn't process message: {payload}");

            if (!robots.TryGetValue(robotId, out var robot))
            {
                robot = new Robot(mqttClient, robotId);
                robots.TryAdd(robotId, robot);
            }

            _ = Task.Run(async () =>
            {
                await robot.MessageRecievedAsync(message, cancellationToken);
            }, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return Task.CompletedTask;
    };

    Console.WriteLine("Subscribing to Topics: IT_to_Fake#");
    await mqttClient.SubscribeAsync("#");

    var fakeCount = Environment.GetEnvironmentVariable("NUMBER_OF_INITIAL_FAKES");
    if (fakeCount != null && int.TryParse(fakeCount, out var fakesToCreate))
    {
        for (int i = 1; i <= fakesToCreate; i++)
        {
            var robotId = $"Fake{i:D3}";
            robots.TryAdd(robotId, new Robot(mqttClient, robotId));
        }
    }

    Console.WriteLine("Fake Robot started. Press any key to stop...");
    
}), cancellationToken);

Console.ReadKey(true);
Console.WriteLine("Stopping Fake Robot");
await cts.CancelAsync();

static void WriteError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(message);
    Console.ResetColor();
}

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

        await Task.Delay((Math.Abs(xCoord-7) * 1000) / 2, cancellationToken);

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