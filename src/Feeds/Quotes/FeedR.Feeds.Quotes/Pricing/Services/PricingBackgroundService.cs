using FeedR.Feeds.Quotes.Pricing.Requests;
using FeedR.Shared.Streaming;
using Serilog;

namespace FeedR.Feeds.Quotes.Pricing.Services;

internal class PricingBackgroundService: BackgroundService
{
    private int _runningStatus;
    private readonly IPricingGenerator _pricingGenerator;
    private readonly PricingRequestsChannel _requestsChannel;
    private readonly IStreamPublisher _streamPublisher;


    public PricingBackgroundService(
        IPricingGenerator pricingGenerator,
        IStreamPublisher streamPublisher,
        PricingRequestsChannel requestsChannel)
    {
        _pricingGenerator = pricingGenerator;
        _requestsChannel = requestsChannel;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Pricing background service has started.");
        await foreach (var request in _requestsChannel.Requests.Reader.ReadAllAsync(stoppingToken))
        {
            Log.Information($"Pricing background service has received the request: '{request.GetType().Name}'.");

            var _ = request switch
            {
                StartPricing => StartGeneratorAsync(),
                StopPricing => StopGeneratorAsync(),
                _ => Task.CompletedTask
            };
        }

        Log.Information("Pricing background service has stopped.");

    }

    private async Task StartGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 1) == 1)
        {
            Log.Information("Pricing generator is already running.");
            return;
        }

        await foreach (var currencyPair in _pricingGenerator.StartAsync())
        {
            Log.Information("Publishing the currency pair...");
            await _streamPublisher.PublishAsync("pricing", currencyPair);
        }
    }

    private async Task StopGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 0) == 0)
        {
            Log.Information("Pricing generator is not running.");
            return;
        }

        await _pricingGenerator.StopAsync();
    }
}