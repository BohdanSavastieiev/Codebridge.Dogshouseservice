using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Models.Common;
using FluentValidation;

namespace Codebridge.TechnicalTask.API.Validators.Common;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithErrorCode(ApiErrorCodes.Pagination.InvalidPageNumber)
            .WithMessage("PageNumber must be greater than or equal to 1");
        
        
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithErrorCode(ApiErrorCodes.Pagination.InvalidPageSize)
            .WithMessage("PageSize must be greater than or equal to 1");
    }
}