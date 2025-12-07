using MediatR;

namespace Application.Features.TempFeatures.Commands.DeleteTemp;
public sealed record DeleteTempCommand() : IRequest<DeleteTempCommandResponse>;
