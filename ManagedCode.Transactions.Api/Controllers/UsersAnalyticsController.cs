using ManagedCode.Transactions.API.Contracts.Requests;
using ManagedCode.Transactions.Api.Services.Abstractions;
using ManagedCode.Transactions.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;

namespace ManagedCode.Transactions.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersAnalyticsController(IUserAnalyticsService service) : HttpController
{
    [HttpGet("report")]
    public async Task<IActionResult> GetReportAsync([FromQuery] GetReportModelRequest request,
        CancellationToken cancellationToken = default)
    {
        var serviceResponse = await service.GetReportAsync(request, cancellationToken);
        return ActionResult(serviceResponse);
    }
}