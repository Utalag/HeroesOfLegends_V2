using HoL.Aplication.DTOs.EntitiDtos;
using MediatR;

namespace HeroesOfLegends.Application.Handlers.Commands.RaceCommand.CreatedRace
{
    /// <summary>
    /// Command pro vytvoření nové rasy. Vrací ID nově vytvořeného záznamu.
    /// </summary>
    
    public sealed record CreatedRaceCommand(RaceDto RaceDto) : IRequest<int>;
}


