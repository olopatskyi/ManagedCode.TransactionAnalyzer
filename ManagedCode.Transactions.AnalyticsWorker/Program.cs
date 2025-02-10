using System.Reflection;
using ManagedCode.Transactions.AnalyticsWorker.Extensions;

var builder = Host.CreateApplicationBuilder(args)
    .AddConfigurations();

builder.Services
    .AddOptions(builder.Configuration)
    .AddDatabase()
    .AddRepositories()
    .AddConsumers()
    .AddServices()
    .AddAutoMapper(Assembly.GetExecutingAssembly());

var host = builder.Build();
host.Run();