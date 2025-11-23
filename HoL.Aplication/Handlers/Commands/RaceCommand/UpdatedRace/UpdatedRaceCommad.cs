using HoL.Aplication.DTOs.EntitiDtos;
using MediatR;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace
{
    public sealed record UpdatedRaceCommand(RaceDto RaceDto) : IRequest<int>;
}