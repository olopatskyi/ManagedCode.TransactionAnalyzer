using FluentValidation.Results;
using ManagedCode.Transactions.Api.Contracts.Requests;
using ManagedCode.Transactions.API.Infrastructure;
using ManagedCode.Transactions.Api.Infrastructure.ClassMappers;
using ManagedCode.Transactions.Api.Infrastructure.Configurations;
using ManagedCode.Transactions.Api.Models;
using ManagedCode.Transactions.Api.Options;
using ManagedCode.Transactions.Api.Repositories.Abstractions;
using ManagedCode.Transactions.Api.Services.Abstractions;
using ManagedCode.Transactions.Infrastructure.Api.Responses;
using ManagedCode.Transactions.Infrastructure.RabbitMq.Abstractions.Producers;
using ManagedCode.Transactions.Infrastructure.Services.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ManagedCode.Transactions.API.Services;

public class TransactionsService(
    IOptions<CsvReaderOptions> csvReaderOptions,
    IRabbitMqProducer rabbitMqProducer,
    ITransactionsRepository transactionsRepository) : ServiceBase, ITransactionsService
{
    private readonly CsvReaderOptions _csvReaderOptions = csvReaderOptions.Value;

    public async Task<ServiceResponse<ValidationResult>> ProcessTransactionsAsync(
        ProcessTransactionsModelRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult =
            await new ProcessTransactionsModelRequestValidator().ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Failure(validationResult);

        var csvReader = new CsvReader<TransactionModel>(request.File.OpenReadStream(), new CsvReaderConfiguration
        {
            BatchSize = _csvReaderOptions.BatchSize,
            Mapper = new TransactionClassMap()
        });

        while (await csvReader.MoveNextAsync(cancellationToken))
        {
            var batch = csvReader.Current.ToList();
            await Parallel.ForEachAsync(
                batch.Chunk(_csvReaderOptions.BatchSize / 10),
                new ParallelOptions { MaxDegreeOfParallelism = 10 },
                async (chunk, token) =>
                {
                    await transactionsRepository.CreateManyAsync(chunk.ToList(), token);
                    await rabbitMqProducer.PublishMessageAsync(
                        JsonConvert.SerializeObject(chunk.Select(x => x.TransactionId)), token);
                }
            );
        }

        return Success();
    }
}