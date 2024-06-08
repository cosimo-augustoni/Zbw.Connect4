
using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis")
    .WithDataVolume("redis-data")
    .WithPersistence(TimeSpan.FromSeconds(2))
    .WithRedisCommander();

var mongoDb = builder.AddMongoDB("mongodb")
    .WithDataVolume("mongodb-data")
    .WithMongoExpress()
    .WithEnvironment("MONGO_INITDB_DATABASE", "projections")
    .AddDatabase("projections");

var orleans = builder
    .AddOrleans("orleans")
    .WithDevelopmentClustering()
    //.WithClustering(redis)
    .WithClusterId("connect4Cluster")
    .WithServiceId("connect4Service")
    .WithGrainStorage("games", redis)
    .WithGrainStorage("visualizers", redis);

var rabbitMQUsername = builder.AddParameter("RabbitMQUsername");
var rabbitMQPassword = builder.AddParameter("RabbitMQPassword", secret: true);

var rabbitMq = builder.AddRabbitMQ("rabbitmq", rabbitMQUsername, rabbitMQPassword)
    .WithArgs("/bin/bash", "-c","rabbitmq-plugins enable --offline rabbitmq_mqtt; rabbitmq-server")
    .WithEndpoint(1883, targetPort: 1883, name: "mqtt")
    .WithManagementPlugin();

var mqttEndpoint = rabbitMq.GetEndpoint("mqtt");

builder.AddProject<Projects.Connect4_Web>("webapp")
    .WithReference(orleans)
    .WithReference(mongoDb)
    .WithEnvironment("MQTT_HOST", $"{mqttEndpoint.Property(EndpointProperty.Host)}")
    .WithEnvironment("MQTT_USERNAME", rabbitMQUsername)
    .WithEnvironment("MQTT_PASSWORD", rabbitMQPassword)
    .WithEnvironment("FRONTEND_LOADING_DELAY", "500");

builder.AddProject<Projects.Connect4_FakeRobot>("fakerobot")
    .WithEnvironment("MQTT_HOST", $"{mqttEndpoint.Property(EndpointProperty.Host)}")
    .WithEnvironment("MQTT_USERNAME", rabbitMQUsername)
    .WithEnvironment("MQTT_PASSWORD", rabbitMQPassword)
    .WithEnvironment("NUMBER_OF_INITIAL_FAKES", "1");

builder.Build().Run();
