using FluentValidation;

namespace Application.Features.TempFeatures.Commands.CreateTemp;
public sealed class CreateTempCommandValidator: AbstractValidator<CreateTempCommand>
{
    public CreateTempCommandValidator()
    {
    }
}
