using MediatR;

namespace Application.Features.TempFeatures.Commands.UpdateTemp;
public class UpdateTempCommandHandler() : IRequestHandler<UpdateTempCommand,UpdateTempCommandResponse>
{
    public async Task<UpdateTempCommandResponse> Handle(UpdateTempCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
