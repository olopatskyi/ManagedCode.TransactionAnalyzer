using ManagedCode.Transactions.AnalyticsWorker.Consumers;
using ManagedCode.Transactions.AnalyticsWorker.Repositories;
using ManagedCode.Transactions.AnalyticsWorker.Repositories.Abstractions;
using ManagedCode.Transactions.AnalyticsWorker.Services;
using ManagedCode.Transactions.AnalyticsWorker.Services.Abstractions;
using ManagedCode.Transactions.Data;
using ManagedCode.Transactions.Infrastructure.MongoDb.Abstractions;
using ManagedCode.Transactions.Infrastructure.RabbitMq.Abstractions;

namespace ManagedCode.Transactions.AnalyticsWorker.Extensions;

public static class ServiceCollectionExtensions
{
    public static HostApplicationBuilder AddConfigurations(this HostApplicationBuilder builder)
    {
        builder.Configuration.
            AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"secrets.json", optional: true)
            .AddEnvironmentVariables();
        
        return builder;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserAnalyticsService, UserAnalyticsService>();
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<ITransactionEntityRepository, TransactionEntityRepository>();
        services.AddSingleton<IUserAnalyticsEntityRepository, UserAnalyticsEntityRepository>();
        
        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbContextOptions>(configuration.GetSection("transactionsDb"));
        services.Configure<RabbitMqConnectionOptions>(configuration.GetSection("rabbitMq"));
        
        return services;
    }

    public static IServiceCollection AddConsumers(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
        services.AddHostedService<TransactionsBatchConsumer>();
        return services;
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<TransactionAnalyzerContext>();
        return services;
    }
}