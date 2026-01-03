using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Responses;
using HoL.Domain.Enums;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetSeqencRaces
{
    

    public sealed record GetRaceSeqencQuery(
        int page =1,
        int size = 5,
        string? SortBy = null,   // povolené: "RaceName", "Id", "RaceCategory"
        SortDirection SortDir = SortDirection.Original
    ) : IRequest<Response<IEnumerable<RaceDto>>>;
}
