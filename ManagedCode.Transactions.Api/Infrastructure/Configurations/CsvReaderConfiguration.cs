using CsvHelper.Configuration;

namespace ManagedCode.Transactions.Api.Infrastructure.Configurations;

public class CsvReaderConfiguration
{
    public int BatchSize { get; set; }
    
    public ClassMap Mapper { get; set; }
}