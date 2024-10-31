using Codebridge.TechnicalTask.Application.Common.Constants;
using Codebridge.TechnicalTask.Application.Common.Extensions;

namespace Codebridge.TechnicalTask.Application.Common.Models;

public record SortParameters
{
    public string PropertyName { get;}
    public SortOrder Order { get;}
    
    public SortParameters(string propertyName, string? order = default)
    {
        PropertyName = propertyName;

        if (!string.IsNullOrEmpty(order))
        {
            var isParsed = SortOrderExtensions.TryParse(order, out SortOrder? sortOrder);

            if (!isParsed)
            {
                throw new InvalidOperationException("Invalid order was passed to the constructor.");
            }
            
            Order = isParsed
                ? sortOrder!.Value
                : SortOrder.Asc;
        }
    }
}