using HoL.Aplication.DTOs.EntitiDtos;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetRaceByIds
{
    public sealed record GetRacesByIdsQuery(IEnumerable<int> Ids) : IRequest<IEnumerable<RaceDto>>;
}
