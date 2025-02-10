using Newtonsoft.Json;

namespace ManagedCode.Transactions.Api.Models;

public class ReportModel
{
    [JsonProperty("users_summary")]
    public List<UserAnalyticsReportSummaryModel> UsersSummary { get; set; }

    [JsonProperty("top_categories")]
    public List<UserAnalyticsReportCategoryModel> TopCategories { get; set; }

    [JsonProperty("highest_spender")]
    public UserAnalyticsReportSpenderModel HighestSpender { get; set; }
}

public class UserAnalyticsReportSummaryModel
{
    [JsonProperty("user_id")]
    public string UserId { get; set; }

    [JsonProperty("total_income")]
    public decimal TotalIncome { get; set; }

    [JsonProperty("total_expense")]
    public decimal TotalExpense { get; set; }
}

public class UserAnalyticsReportCategoryModel
{
    [JsonProperty("category")]
    public string Category { get; set; }

    [JsonProperty("transactions_count")]
    public int TransactionsCount { get; set; }
}

public class UserAnalyticsReportSpenderModel
{
    [JsonProperty("user_id")]
    public string UserId { get; set; }

    [JsonProperty("total_expense")]
    public decimal TotalExpense { get; set; }
}