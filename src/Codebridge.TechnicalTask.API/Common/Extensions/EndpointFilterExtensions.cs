using Codebridge.TechnicalTask.API.Common.Filters;

namespace Codebridge.TechnicalTask.API.Common.Extensions;

public static class EndpointFilterExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(
        this RouteHandlerBuilder builder) where T : class
    {
        return builder.AddEndpointFilter<ValidatorFilter<T>>();
    }
}