using MediatR;

namespace Application.Features.TempFeatures.Queries.GetTempById;
public sealed record GetTempByIdQuery() : IRequest<GetTempByIdQueryResponse>;
