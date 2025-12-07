using MediatR;

namespace Application.Features.TempFeatures.Queries.GetAllTemp;
public sealed record GetAllTempQuery() : IRequest<GetAllTempQueryResponse>;
