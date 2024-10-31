namespace Codebridge.TechnicalTask.API.Common.Constants;

public static class ApiErrorCodes
{
    public const string InvalidRequest = "Request.Invalid";
    public const string ConfigurationMissing = "Configuration.Missing";
    public const string TooManyRequests = "RateLimit.Exceeded";

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