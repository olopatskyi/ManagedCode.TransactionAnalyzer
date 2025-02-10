namespace ManagedCode.Transactions.AnalyticsWorker.Models.Requests;

public class CreateAnalyticsModelRequest
{
    public List<Guid> TransactionsIds { get; set; }
}