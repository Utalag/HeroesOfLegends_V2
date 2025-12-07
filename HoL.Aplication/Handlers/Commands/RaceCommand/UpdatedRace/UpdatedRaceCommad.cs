using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Responses;
using MediatR;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace
{
    public sealed record UpdatedRaceCommand(RaceDto RaceDto) : IRequest<Response<int>>;
}