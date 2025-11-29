using AutoMapper;
using HeroesOfLegends.Application.Handlers.Commands.RaceCommand.CreatedRace;
using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.RemoveRace
{
    public class RemoveRaceHandler : IRequestHandler<RemoveRaceCommand, bool>
    {
        private readonly IRaceRepository _repository;
        private readonly ILogger<RemoveRaceHandler> _logger;

        public RemoveRaceHandler(
            IRaceRepository repository,
            ILogger<RemoveRaceHandler> logger,
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(RemoveRaceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Remove Race with Id: {RaceId}, Name: {RaceName}, Category: {RaceCategory}",
                request.RaceDto.RaceId,
                request.RaceDto.RaceName,
                request.RaceDto.RaceCategory);

            try
            {
                var existingRace = await _repository.ExistsAsync(request.RaceDto.RaceId, cancellationToken);

                if (existingRace)
                {
                    _logger.LogInformation("Race with Id is yet Removed: {RaceId}, Name: {RaceName}",
                        request.RaceDto.RaceId, request.RaceDto.RaceName);
                    return true;
                }
                // removed
                await _repository.DeleteAsync(request.RaceDto.RaceId,cancellationToken);
                _logger.LogInformation("Race with Id is Removed: {RaceId}, Name: {RaceName}",
                        request.RaceDto.RaceId, request.RaceDto.RaceName);
                return true;

            }
            catch (Exception ex)
            {
                // Logování neočekávaných chyb
                _logger.LogError(
                    ex,
                    "Error removed Race with Id: {RaceId}, Name: {RaceName}",
                    request.RaceDto.RaceId,
                    request.RaceDto.RaceName);
                throw; // Re-throw pro vyšší vrstvy (např. API middleware)
            }
        }
    }
}
