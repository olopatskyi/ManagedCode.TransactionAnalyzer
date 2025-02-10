namespace ManagedCode.Transactions.API.Infrastructure.Abstractions;

public interface ICsvReader<TCsvModel>
{
    Task<bool> MoveNextAsync(CancellationToken cancellationToken = default);
    
    IEnumerable<TCsvModel> Current { get; }
}