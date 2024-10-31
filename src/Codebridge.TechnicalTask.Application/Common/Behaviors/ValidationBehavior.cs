using Codebridge.TechnicalTask.Domain.Shared.Models;
using FluentValidation;
using MediatR;

namespace Codebridge.TechnicalTask.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IValidator<TRequest>[] _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators.ToArray();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Length == 0)
        {
            return await next();
        }

        var validationResults = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Select(failure => Error.Validation(
                failure.ErrorCode,
                failure.ErrorMessage))
            .ToList();

        if (validationResults.Count == 0)
        {
            return await next();
        }

        return CreateValidationResult(validationResults);
    }

    private static TResponse CreateValidationResult(List<Error> errors)
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)Result.Failure(errors);
        }
        
        var genericType = typeof(TResponse).GetGenericArguments()[0];
        var method = typeof(Result)
            .GetMethods()
            .First(m => 
                m.Name == nameof(Result.Failure) && 
                m.IsGenericMethod &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType == typeof(IEnumerable<Error>));

        var genericMethod = method.MakeGenericMethod(genericType);
        return (TResponse)genericMethod.Invoke(null, [errors])!;
    }
}