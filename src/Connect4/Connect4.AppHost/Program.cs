using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis", 6381)
    .WithDataVolume("redis-data")
    .WithPersistence(TimeSpan.FromSeconds(2))
    .WithRedisCommander();

var mongoDb = builder.AddMongoDB("mongodb")
    .WithDataVolume("mongodb-data")
    .WithMongoExpress()
    .AddDatabase("projections");

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
    .WithReference(orleans)
    .WithReference(mongoDb);

builder.Build().Run();
