using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis")
    .WithDataVolume("redis-data")
    .WithPersistence(TimeSpan.FromSeconds(2))
    .WithRedisCommander();

var mongoDb = builder.AddMongoDB("mongodb")
    .WithDataVolume("mongodb-data")
    .WithMongoExpress()
    .AddDatabase("projections");

var orleans = builder
    .AddOrleans("orleans")
    .WithClustering(redis)
    .WithGrainStorage("games", redis);

builder.AddProject<Projects.Connect4_Web>("webapp")
    .WithReference(orleans)
    .WithReference(mongoDb);

builder.Build().Run();
