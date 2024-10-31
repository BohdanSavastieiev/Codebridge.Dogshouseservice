namespace Codebridge.TechnicalTask.Application.Common.Constants;

public class ApplicationErrorCodes
{
    public static class Pagination
    {
        public const string InvalidPageNumber = "Pagination.InvalidPageNumber";
        public const string InvalidPageSize = "Pagination.InvalidPageSize";
    }
    
    public static class Sort
    {
        public const string AttributeRequired = "Sort.AttributeRequired";
        public const string InvalidOrder = "Sort.InvalidOrder";
    }
}