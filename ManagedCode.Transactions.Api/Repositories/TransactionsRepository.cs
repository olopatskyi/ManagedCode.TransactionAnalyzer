using AutoMapper;
using ManagedCode.Transactions.Api.Models;
using ManagedCode.Transactions.Api.Repositories.Abstractions;
using ManagedCode.Transactions.Data;
using ManagedCode.Transactions.Data.Entities;
using MongoDB.Driver;

namespace ManagedCode.Transactions.Api.Repositories;

public class TransactionsRepository(TransactionAnalyzerContext context, IMapper mapper) : ITransactionsRepository
{
    private readonly IMongoCollection<TransactionEntity> _collection = context.Transactions;

    public Task CreateManyAsync(IReadOnlyCollection<TransactionModel> models,
        CancellationToken cancellationToken = default)
    {
        var entities = mapper.Map<List<TransactionEntity>>(models);
        return _collection.InsertManyAsync(entities,
            new InsertManyOptions
            {
                IsOrdered = false
            },
            cancellationToken: cancellationToken);
    }
}