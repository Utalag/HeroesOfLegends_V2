using FluentValidation;

namespace Application.Features.TempFeatures.Commands.UpdateTemp;
public sealed class UpdateTempCommandValidator: AbstractValidator<UpdateTempCommand>
{
    public UpdateTempCommandValidator()
    {
    }
}
