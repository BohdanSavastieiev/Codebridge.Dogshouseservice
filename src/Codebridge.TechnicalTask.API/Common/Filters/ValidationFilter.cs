using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Common.Factories;
using Codebridge.TechnicalTask.Domain.Shared.Models;
using FluentValidation;

namespace Codebridge.TechnicalTask.API.Common.Filters;

public class ValidatorFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidatorFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var parameter = context.Arguments
            .OfType<T>()
            .FirstOrDefault();

        if (parameter is null)
        {
            var error = Error.Validation(ApiErrorCodes.InvalidRequest,
                "Request body is missing or could not be parsed.");
            
            return Results.Problem(ProblemDetailsFactory.CreateBadRequestProblemDetails(error));
        }

        var validationResult = await _validator.ValidateAsync(parameter);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(x => Error.Validation(x.ErrorCode, x.ErrorMessage))
                .ToList();

            return Results.Problem(
                ProblemDetailsFactory.CreateBadRequestProblemDetails(errors));
        }

        return await next(context);
    }
}