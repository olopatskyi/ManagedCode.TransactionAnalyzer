using ManagedCode.Transactions.Data.Models;

namespace ManagedCode.Transactions.AnalyticsWorker.Models;

public class UserAnalyticsModel
{
    public Guid UserId { get; set; }
    
    public decimal TotalIncome { get; set; }

    public decimal TotalExpense { get; set; }

    public Dictionary<string, int> CategoryCounts { get; set; } = new();

    public TopCategoryModel TopCategory { get; set; }

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}