using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ManagedCode.Transactions.Infrastructure.RabbitMq.Abstractions;

public class RabbitMqConnection : IRabbitMqConnection, IAsyncDisposable
{
    private readonly IOptions<RabbitMqConnectionOptions> _options;
    private readonly Task<IConnection> _connection;
    private readonly Task<IChannel> _channel;

    public RabbitMqConnection(IOptions<RabbitMqConnectionOptions> options)
    {
        _options = options;
        var factory = new ConnectionFactory
        {
            HostName = options.Value.Host,
            UserName = options.Value.Username,
            Password = options.Value.Password
        };

        _connection = factory.CreateConnectionAsync();
        _channel = _connection.ContinueWith(conn => conn.Result.CreateChannelAsync()).Unwrap();

        _channel.ContinueWith(async ch =>
        {
            await ch.Result.QueueDeclareAsync(
                queue: options.Value.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        });
    }

    public Task<IChannel> CreateChannelAsync() => _channel;

    public RabbitMqConnectionOptions Options => _options.Value;

    public async ValueTask DisposeAsync()
    {
        if (_channel.IsCompletedSuccessfully)
        {
            await _channel.Result.CloseAsync();
        }

        if (_connection.IsCompletedSuccessfully)
        {
            await _connection.Result.CloseAsync();
        }
    }
}