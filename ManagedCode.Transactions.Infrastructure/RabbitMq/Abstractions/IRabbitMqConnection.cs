using RabbitMQ.Client;

namespace ManagedCode.Transactions.Infrastructure.RabbitMq.Abstractions;

public interface IRabbitMqConnection
{
    Task<IChannel> CreateChannelAsync();
    
    RabbitMqConnectionOptions Options { get; }
}