using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Aplication.DTOs.EntitiDtos;
using MediatR;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.RemoveRace
{
    public sealed record RemoveRaceCommand(RaceDto RaceDto) : IRequest<bool>;
}
