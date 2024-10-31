using System.Reflection;
using Codebridge.TechnicalTask.Application.Common.Extensions;
using Codebridge.TechnicalTask.Domain.Common.Constants;
using Codebridge.TechnicalTask.Domain.Dogs.Entities;
using FluentValidation;

namespace Codebridge.TechnicalTask.Application.Dogs.Queries.GetDogs;

public class GetDogsQueryValidator : AbstractValidator<GetDogsQuery>
{
    private static readonly HashSet<string?> ValidSortProperties = typeof(Dog)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Select(p => p.Name.ToLowerSnakeCase())
        .ToHashSet();

    public GetDogsQueryValidator()
    {
        When(x => x.SortParameters != null, () =>
        {
            RuleFor(x => x.SortParameters!.PropertyName)
                .Must(attr => ValidSortProperties.Contains(attr.ToLowerSnakeCase()))
                .WithErrorCode(DomainErrorCodes.Dog.Validation.InvalidSortProperty)
                .WithMessage("Invalid sort attribute");
        });
    }
}