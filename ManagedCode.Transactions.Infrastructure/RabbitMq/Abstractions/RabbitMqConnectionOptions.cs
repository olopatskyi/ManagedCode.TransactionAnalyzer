namespace ManagedCode.Transactions.Infrastructure.RabbitMq.Abstractions;

public class RabbitMqConnectionOptions
{
    public string Host { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string QueueName { get; set; }
}