using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Models.Common;
using Codebridge.TechnicalTask.Application.Common.Extensions;
using FluentValidation;

namespace Codebridge.TechnicalTask.API.Validators.Common;

public class SortRequestValidator : AbstractValidator<SortRequest>
{
    public SortRequestValidator()
    {
        RuleFor(x => x.Attribute)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Order))
            .WithErrorCode(ApiErrorCodes.Sort.AttributeRequired)
            .WithMessage("Sort attribute must be provided when sort order is specified");
        
        RuleFor(x => x.Order)
            .Must(x => string.IsNullOrEmpty(x) || SortOrderExtensions.IsValid(x))
            .WithErrorCode(ApiErrorCodes.Sort.InvalidOrder)
            .WithMessage($"Valid values for sort order are: {SortOrderExtensions.GetAllowedOrdersString()}");
    }
}