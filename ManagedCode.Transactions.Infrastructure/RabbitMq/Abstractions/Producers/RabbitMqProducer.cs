using System.Text;
using RabbitMQ.Client;

namespace ManagedCode.Transactions.Infrastructure.RabbitMq.Abstractions.Producers;

public class RabbitMqProducer(IRabbitMqConnection rabbitMqConnection) : IRabbitMqProducer
{
    private IChannel _channel;

    private async Task InitializeChannelAsync()
    {
        _channel = await rabbitMqConnection.CreateChannelAsync();
    }

    public async Task PublishMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        if (_channel == null || _channel.IsClosed)
        {
            await InitializeChannelAsync();
        }

        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await _channel!.BasicPublishAsync(
            exchange: "",
            routingKey: rabbitMqConnection.Options.QueueName,
            mandatory: false,
            basicProperties: properties,
            body: body, cancellationToken: cancellationToken);
    }
}