using MediatR;

namespace Application.Features.TempFeatures.Commands.CreateTemp;
public sealed record CreateTempCommand() : IRequest<CreateTempCommandResponse>;
