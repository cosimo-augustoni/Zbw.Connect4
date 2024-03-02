using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedisContainer("redis", 6381)
    .WithRedisCommander();

if (!builder.Environment.IsDevelopment())
{
    var redisReplica = builder.AddContainer("redis-replica1", "redis", "latest")
        .WithArgs("redis-server", "--slaveof", "redis", "6379", "--port", "6380");
}

var orleans = builder
    .AddOrleans("orleans")
    .WithClustering(redis)
    .WithGrainStorage("games", redis);

builder.AddProject<Projects.Connect4_Web>("webapp")
    .WithReference(orleans);

builder.Build().Run();
