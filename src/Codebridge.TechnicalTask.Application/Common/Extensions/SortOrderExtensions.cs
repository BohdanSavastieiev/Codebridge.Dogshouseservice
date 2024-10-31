using Codebridge.TechnicalTask.Application.Common.Constants;

namespace Codebridge.TechnicalTask.Application.Common.Extensions;

public static class SortOrderExtensions
{
    private static IEnumerable<string> AllowedOrders { get; } = Enum.GetNames<SortOrder>()
        .Select(o => o.ToLower());
    
    public static string GetAllowedOrdersString() => 
        string.Join(", ", AllowedOrders);
    
    public static bool IsValid(string str) => 
        AllowedOrders.Contains(str, StringComparer.OrdinalIgnoreCase);

    public static bool TryParse(string str, out SortOrder? sortOrder)
    {
        switch (str.ToLower())
        {
            case "asc":
                sortOrder = SortOrder.Asc;
                return true;
            case "desc":
                sortOrder = SortOrder.Desc;
                return true;
            default:
                sortOrder = null;
                return false;
        }
    }
}