using MediatR;

namespace Application.Features.TempFeatures.Commands.CreateTemp;
public class CreateTempCommandHandler() : IRequestHandler<CreateTempCommand,CreateTempCommandResponse>
{
    public async Task<CreateTempCommandResponse> Handle(CreateTempCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
