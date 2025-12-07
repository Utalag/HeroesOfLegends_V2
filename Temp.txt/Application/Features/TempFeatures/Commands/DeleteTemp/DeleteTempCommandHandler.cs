using MediatR;

namespace Application.Features.TempFeatures.Commands.DeleteTemp;
public class DeleteTempCommandHandler() : IRequestHandler<DeleteTempCommand,DeleteTempCommandResponse>
{
    public async Task<DeleteTempCommandResponse> Handle(DeleteTempCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
