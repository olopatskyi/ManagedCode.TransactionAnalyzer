using ManagedCode.Transactions.Api.Options;
using ManagedCode.Transactions.Api.Repositories;
using ManagedCode.Transactions.API.Repositories;
using ManagedCode.Transactions.Api.Repositories.Abstractions;
using ManagedCode.Transactions.API.Services;
using ManagedCode.Transactions.Api.Services.Abstractions;
using ManagedCode.Transactions.Data;
using ManagedCode.Transactions.Infrastructure.MongoDb.Abstractions;
using ManagedCode.Transactions.Infrastructure.RabbitMq.Abstractions;
using ManagedCode.Transactions.Infrastructure.RabbitMq.Abstractions.Producers;

namespace ManagedCode.Transactions.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
    {
        builder.Configuration.
            AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"secrets.json", optional: true)
            .AddEnvironmentVariables();

        return builder;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITransactionsService, TransactionsService>();
        services.AddScoped<IUserAnalyticsService, UserAnalyticsService>();
        
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITransactionsRepository, TransactionsRepository>();
        services.AddScoped<IUserAnalyticsRepository, UserAnalyticsRepository>();
        
        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbContextOptions>(configuration.GetSection("transactionsDb"));
        services.Configure<CsvReaderOptions>(configuration.GetSection("csvReaderOptions"));
        services.Configure<RabbitMqConnectionOptions>(configuration.GetSection("rabbitMq"));
        
        return services;
    }

    public static IServiceCollection AddProducers(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();
        services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
        return services;
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<TransactionAnalyzerContext>();
        return services;
    }
}