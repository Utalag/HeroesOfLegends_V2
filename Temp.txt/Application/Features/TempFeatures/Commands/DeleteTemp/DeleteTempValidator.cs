using FluentValidation;

namespace Application.Features.TempFeatures.Commands.DeleteTemp;
public sealed class DeleteTempCommandValidator: AbstractValidator<DeleteTempCommand>
{
    public DeleteTempCommandValidator()
    {
    }
}
