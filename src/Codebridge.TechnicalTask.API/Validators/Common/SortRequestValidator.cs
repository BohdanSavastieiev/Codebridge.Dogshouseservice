using Codebridge.TechnicalTask.API.Models.Common;
using Codebridge.TechnicalTask.Application.Common.Constants;
using Codebridge.TechnicalTask.Application.Common.Extensions;
using FluentValidation;

namespace Codebridge.TechnicalTask.API.Validators.Common;

public class SortRequestValidator : AbstractValidator<SortRequest>
{
    public SortRequestValidator()
    {
        RuleFor(x => x.Attribute)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .When(x => !string.IsNullOrWhiteSpace(x.Order))            
            .WithErrorCode(ApplicationErrorCodes.Sort.AttributeRequired)
            .WithMessage("Sort attribute must be provided when sort order is specified");
        
        RuleFor(x => x.Order)
            .Must(x => string.IsNullOrWhiteSpace(x) || SortOrderExtensions.IsValid(x))
            .WithErrorCode(ApplicationErrorCodes.Sort.InvalidOrder)
            .WithMessage($"Valid values for sort order are: {SortOrderExtensions.GetAllowedOrdersString()}");
    }
}