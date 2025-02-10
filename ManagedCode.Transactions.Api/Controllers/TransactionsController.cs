using ManagedCode.Transactions.Api.Contracts.Requests;
using ManagedCode.Transactions.Infrastructure.Api;
using ManagedCode.Transactions.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ManagedCode.Transactions.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController(ITransactionsService transactionsService) : HttpController
{
    [HttpPost]
    public async Task<IActionResult> UploadTransactionsAsync([FromForm] ProcessTransactionsModelRequest request,
        CancellationToken cancellationToken = default)
    {
        var serviceResponse = await transactionsService.ProcessTransactionsAsync(new ProcessTransactionsModelRequest
        {
            File = request.File,
        }, cancellationToken);

        return ActionResult(serviceResponse);
    }
}