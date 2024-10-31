using Codebridge.TechnicalTask.Application.Common.Constants;
using Codebridge.TechnicalTask.Application.Common.Extensions;
using Codebridge.TechnicalTask.Domain.Shared.Models;

namespace Codebridge.TechnicalTask.Application.Common.Models;

public record SortParameters
{
    public string PropertyName { get;}
    public SortOrder Order { get;}
    
    private SortParameters(string propertyName, SortOrder order = SortOrder.Asc)
    {
        PropertyName = propertyName;
        Order = order;
    }

    public static Result<SortParameters> Create(string propertyName, string? order = default)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return Result.Failure<SortParameters>(
                Error.Validation(
                    ApplicationErrorCodes.Sort.AttributeRequired,
                "Invalid property name was provided."));
        }

        SortOrder finalOrder = SortOrder.Asc;
        if (!string.IsNullOrWhiteSpace(order))
        {
            var isParsed = SortOrderExtensions.TryParse(order, out SortOrder? sortOrder);

            if (!isParsed)
            {
                return Result.Failure<SortParameters>(
                    Error.Validation(
                        ApplicationErrorCodes.Sort.InvalidOrder,
                        "Invalid order was provided."));            
            }
            
            finalOrder = sortOrder!.Value;
        }
        
        return Result.Success(new SortParameters(propertyName.Trim(), finalOrder));
    }
}