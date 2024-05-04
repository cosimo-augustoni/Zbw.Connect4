using Connect4.Backend;
using Connect4.Frontend;
using Connect4.ServiceDefaults;
using Orleans.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddKeyedRedisClient("redis");
builder.UseOrleans(siloBuilder =>
{
    siloBuilder.AddLogStorageBasedLogConsistencyProvider();
    siloBuilder.Services.AddSerializer(serializerBuilder =>
    {
        serializerBuilder.AddJsonSerializer(_ => true);
    });
});
builder.AddMongoDBClient("projections");

builder.Services.AddBackend();
builder.Services.AddFrontend(builder.Configuration.GetSection("AzureAd"));

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseFrontend();

app.Run();
