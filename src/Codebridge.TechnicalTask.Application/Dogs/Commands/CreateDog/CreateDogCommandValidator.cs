using Codebridge.TechnicalTask.Domain.Common.Constants;
using FluentValidation;

namespace Codebridge.TechnicalTask.Application.Dogs.Commands.CreateDog;

public class CreateDogCommandValidator : AbstractValidator<CreateDogCommand>
{
    public CreateDogCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(DomainConstants.Dog.MaxNameLength)
            .WithErrorCode(DomainErrorCodes.Dog.Validation.InvalidNameLength);
        
        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(DomainConstants.Dog.MaxColorLength)
            .WithErrorCode(DomainErrorCodes.Dog.Validation.InvalidColorLength);
        
        RuleFor(x => x.TailLength)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(DomainErrorCodes.Dog.Validation.InvalidTailLength);
        
        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .WithErrorCode(DomainErrorCodes.Dog.Validation.InvalidWeight);
    }
}