using Microsoft.AspNetCore.Mvc;

namespace ManagedCode.Transactions.API.Contracts.Requests;

public class GetReportModelRequest
{
    [FromQuery(Name = "s")]
    public int Skip { get; set; } = 0;

    [FromQuery(Name = "l")]
    public int Limit { get; set; } = 50;
}