namespace CaravelTemplate.Api.Shared.Messaging;

public interface IPublisher
{
    Task PublishAsync<T>(T message, CancellationToken ct) where T : IMessage;
}