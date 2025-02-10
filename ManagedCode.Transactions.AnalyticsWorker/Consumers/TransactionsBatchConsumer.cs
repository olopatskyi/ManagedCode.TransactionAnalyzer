using System.Text;
using ManagedCode.Transactions.AnalyticsWorker.Models.Requests;
using ManagedCode.Transactions.AnalyticsWorker.Services.Abstractions;
using ManagedCode.Transactions.Infrastructure.RabbitMq.Abstractions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ManagedCode.Transactions.AnalyticsWorker.Consumers;

public class TransactionsBatchConsumer(
    IRabbitMqConnection rabbitMqConnection,
    IUserAnalyticsService service) : BackgroundService
{
    private IChannel _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await rabbitMqConnection.CreateChannelAsync();
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await service.CreateAnalyticsAsync(new CreateAnalyticsModelRequest
            {
                TransactionsIds = JsonConvert.DeserializeObject<List<Guid>>(message)
            }, stoppingToken);
            
            await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await _channel.BasicConsumeAsync(
            queue: rabbitMqConnection.Options.QueueName,
            autoAck: false,
            consumer: consumer, cancellationToken: stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
        {
            await _channel.CloseAsync(cancellationToken: cancellationToken);
        }

        Console.WriteLine("RabbitMQ Consumer stopped.");
    }
}