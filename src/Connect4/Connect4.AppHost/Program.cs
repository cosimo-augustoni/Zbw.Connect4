using Aspire.Hosting;
using Microsoft.Extensions.Hosting;

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
    .WithClustering(redis)
    .WithGrainStorage("games", redis)
    .WithGrainStorage("visualizers", redis);

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithArgs("/bin/bash", "-c","rabbitmq-plugins enable --offline rabbitmq_mqtt; rabbitmq-server")
    .WithManagementPlugin();

builder.AddProject<Projects.Connect4_Web>("webapp")
    .WithReference(orleans)
    .WithReference(mongoDb)
    .WithReference(rabbitMq);

builder.Build().Run();
