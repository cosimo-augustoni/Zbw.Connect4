using Connect4.Backend;
using Connect4.Frontend;
using Connect4.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddKeyedRedisClient("redis");
builder.UseOrleans(siloBuilder =>
{
    siloBuilder.AddLogStorageBasedLogConsistencyProvider();
});
builder.AddMongoDBClient("projections");

builder.Services.AddBackend();
builder.Services.AddFrontend(builder.Configuration.GetSection("AzureAd"));

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseFrontend();

app.Run();
