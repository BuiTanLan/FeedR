using FeedR.Aggregator.Services;
using FeedR.Shared.Redis;
using FeedR.Shared.Redis.Streaming;
using FeedR.Shared.Serialization;
using FeedR.Shared.Streaming;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHostedService<PricingStreamBackgroundService>()
    .AddSerialization()
    .AddStreaming()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming();

builder.Host.UseSerilog((_, lc) => lc.WriteTo.Console());
var app = builder.Build();

app.MapGet("/", () => "FeedR Aggregator!");

app.Run();