using Codebridge.TechnicalTask.API.Models.Common;
using Codebridge.TechnicalTask.Application.Common.Constants;
using FluentValidation;

namespace Codebridge.TechnicalTask.API.Validators.Common;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithErrorCode(ApplicationErrorCodes.Pagination.InvalidPageNumber)
            .WithMessage("PageNumber must be greater than or equal to 1");
        
        
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithErrorCode(ApplicationErrorCodes.Pagination.InvalidPageSize)
            .WithMessage("PageSize must be greater than or equal to 1");
    }
}