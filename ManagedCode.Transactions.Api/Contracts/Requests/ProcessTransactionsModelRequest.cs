using FluentValidation;
using ManagedCode.Transactions.Infrastructure.Api.Extensions;

namespace ManagedCode.Transactions.Api.Contracts.Requests;

public class ProcessTransactionsModelRequest
{
    public IFormFile File { get; set; }
}

public class ProcessTransactionsModelRequestValidator : AbstractValidator<ProcessTransactionsModelRequest>
{
    public ProcessTransactionsModelRequestValidator()
    {
        RuleFor(x => x.File)
            .Required();
    }
}