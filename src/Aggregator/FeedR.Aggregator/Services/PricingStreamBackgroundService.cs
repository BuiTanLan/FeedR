using FeedR.Shared.Streaming;
using Serilog;

namespace FeedR.Aggregator.Services;

internal class PricingStreamBackgroundService: BackgroundService
{
    private readonly IStreamSubscriber _subscriber;

    public PricingStreamBackgroundService(IStreamSubscriber subscriber, ILogger<PricingStreamBackgroundService> logger)
    {
        _subscriber = subscriber;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _subscriber.SubscribeAsync<CurrencyPair>("pricing", currencyPair =>
        {
            Log.Information($"Pricing '{currencyPair.Symbol}' = {currencyPair.Value:F}, " +
                                   $"timestamp: {currencyPair.Timestamp}");
        });
    }

    private record CurrencyPair(string Symbol, decimal Value, long Timestamp);
}