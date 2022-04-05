namespace FeedR.Shared.Streaming;

public sealed class DefaultStreamSubscriber: IStreamSubscriber
{
    public Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class 
        => Task.CompletedTask;
}