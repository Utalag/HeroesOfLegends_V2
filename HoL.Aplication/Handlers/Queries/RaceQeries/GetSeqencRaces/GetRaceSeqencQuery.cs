using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Domain.Enums;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetSeqencRaces
{
    

    // Sekvenční načítání ras s podporou stránkování a řazení

    /// <summary>
    /// Represents a query for retrieving a paginated and optionally sorted sequence of races.
    /// </summary>
    /// <remarks>Use this query to efficiently retrieve race data in pages, with optional sorting by supported
    /// properties. This is useful for scenarios such as displaying race lists in a user interface with pagination and
    /// sorting controls.</remarks>
    /// <param name="page">The zero-based index of the page to retrieve. Must be greater than or equal to 0.</param>
    /// <param name="size">The maximum number of races to include in the returned page. Must be greater than 0.</param>
    /// <param name="SortBy">The name of the property to sort the results by. Allowed values are "RaceName", "RaceId", or "RaceCategory". If
    /// null, the default sorting is applied.</param>
    /// <param name="SortDir">The direction in which to sort the results. Allowed values are <b>"asc"</b> for ascending or <b>"desc"</b> for descending. If
    /// null, the default direction is used.</param>
    public sealed record GetRaceSeqencQuery(
        int page =1,
        int size = 5,
        string? SortBy = null,   // povolené: "RaceName", "RaceId", "RaceCategory"
        SortDirection SortDir = SortDirection.Original
    ) : IRequest<IEnumerable<RaceDto>>;
}
