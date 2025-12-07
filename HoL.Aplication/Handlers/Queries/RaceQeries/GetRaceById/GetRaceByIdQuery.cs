using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Responses;
using MediatR;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetRaceById
{

    public sealed record GetRaceByIdQuery(int Id) : IRequest<Response<RaceDto?>>;
}
