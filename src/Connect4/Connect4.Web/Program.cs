using Connect4.Frontend;
using Connect4.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddFrontend(builder.Configuration.GetSection("AzureAd"));

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseFrontend();

app.Run();
