using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Models.Dogs;
using FluentValidation;

namespace Codebridge.TechnicalTask.API.Validators.Dogs;

public class CreateDogRequestValidator : AbstractValidator<CreateDogRequest>
{
    public CreateDogRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(ApiErrorCodes.InvalidRequest);

        RuleFor(x => x.Color)
            .NotEmpty()
            .WithErrorCode(ApiErrorCodes.InvalidRequest);

        RuleFor(x => x.TailLength)
            .NotNull()
            .WithErrorCode(ApiErrorCodes.InvalidRequest);

        RuleFor(x => x.Weight)
            .NotNull()
            .WithErrorCode(ApiErrorCodes.InvalidRequest);
    }
}