using MediatR;

namespace Application.Features.TempFeatures.Commands.UpdateTemp;
public sealed record UpdateTempCommand() : IRequest<UpdateTempCommandResponse>;
