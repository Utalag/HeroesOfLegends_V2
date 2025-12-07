using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Responses;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.AllRaces
{
    public sealed record GetRaceAllQuery() : IRequest<Response<IEnumerable<RaceDto>>>;
}
