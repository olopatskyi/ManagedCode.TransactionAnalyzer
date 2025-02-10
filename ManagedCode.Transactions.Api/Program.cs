using System.Reflection;
using ManagedCode.Transactions.API.Extensions;
using ManagedCode.Transactions.Data;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args)
    .AddConfigurations();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 500 * 1024 * 1024; //500mb
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // 500MB
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions(builder.Configuration)
    .AddDatabase()
    .AddRepositories()
    .AddServices()
    .AddProducers()
    .AddAutoMapper(Assembly.GetExecutingAssembly());


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.MapControllers();

await app.Services.GetRequiredService<TransactionAnalyzerContext>().MigrateAsync();
app.Run();