using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ManagedCode.Transactions.API.Infrastructure.Abstractions;
using ManagedCode.Transactions.Api.Infrastructure.Configurations;

namespace ManagedCode.Transactions.API.Infrastructure;

public class CsvReader<TCsvModel> : ICsvReader<TCsvModel>
{
    private readonly StreamReader _streamReader;
    private readonly CsvReader _csvReader;
    private readonly int _batchSize;
    private bool _isDisposed;
    private IAsyncEnumerator<TCsvModel>? _enumerator;

    public CsvReader(Stream fileStream, CsvReaderConfiguration configuration)
    {
        _batchSize = configuration.BatchSize;
        _streamReader = new StreamReader(fileStream, leaveOpen: true);
        _csvReader = new CsvReader(_streamReader, new CsvConfiguration(CultureInfo.InvariantCulture));
        _csvReader.Context.RegisterClassMap(configuration.Mapper);
        _enumerator = _csvReader.GetRecordsAsync<TCsvModel>().GetAsyncEnumerator();
    }

    public IEnumerable<TCsvModel> Current { get; private set; } = new List<TCsvModel>();

    public async Task<bool> MoveNextAsync(CancellationToken cancellationToken = default)
    {
        var records = new List<TCsvModel>();

        for (int i = 0; i < _batchSize; i++)
        {
            if (!await _enumerator!.MoveNextAsync()) break;
            records.Add(_enumerator.Current);
        }

        if (records.Count == 0)
        {
            return false;
        }

        Current = records;
        return true;
    }
}