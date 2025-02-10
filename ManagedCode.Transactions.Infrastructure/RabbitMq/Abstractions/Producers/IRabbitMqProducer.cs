namespace ManagedCode.Transactions.Infrastructure.RabbitMq.Abstractions.Producers;

public interface IRabbitMqProducer
{
    Task PublishMessageAsync(string message, CancellationToken cancellationToken = default);
}