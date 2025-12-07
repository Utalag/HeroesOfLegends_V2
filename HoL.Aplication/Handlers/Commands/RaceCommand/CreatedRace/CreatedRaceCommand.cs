using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Responses;
using MediatR;

namespace HeroesOfLegends.Application.Handlers.Commands.RaceCommand.CreatedRace
{
    /// <summary>
    /// Command pro vytvoření nové rasy. Vrací Response s ID nově vytvořeného záznamu.
    /// </summary>
    public sealed record CreatedRaceCommand(RaceDto RaceDto) : IRequest<Response<int>>;
}


