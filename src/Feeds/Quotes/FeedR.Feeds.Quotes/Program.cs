using FeedR.Feeds.Quotes.Pricing.Requests;
using FeedR.Feeds.Quotes.Pricing.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, lc) => lc.WriteTo.Console());
builder.Services
    .AddSingleton<PricingRequestsChannel>()
    .AddSingleton<IPricingGenerator, PricingGenerator>()
    .AddHostedService<PricingBackgroundService>();


var app = builder.Build();

app.MapGet("/", () => "FeedR Quotes feed");
app.MapPost("/pricing/start", async (PricingRequestsChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StartPricing());
    return Results.Ok();
});

app.MapPost("/pricing/stop", async (PricingRequestsChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StopPricing());
    return Results.Ok();
});


app.Run();