using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Responses;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetRaceByIds
{
    public sealed record GetRacesByIdsQuery(IEnumerable<int> Ids) : IRequest<Response<IEnumerable<RaceDto>>>;
}
