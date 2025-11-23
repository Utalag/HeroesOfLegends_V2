using AutoMapper;
using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace
{
    /// <summary>
    /// Handler pro aktualizaci existující rasy.
    /// Validace probíhá automaticky přes FluentValidation pipeline (ValidationBehavior).
    /// </summary>
    public class UpdatedRaceHandler : IRequestHandler<UpdatedRaceCommand, int>
    {
        private readonly IRaceRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatedRaceHandler> _logger;

        public UpdatedRaceHandler(
            IRaceRepository repository,
            IMapper mapper,
            ILogger<UpdatedRaceHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(UpdatedRaceCommand request, CancellationToken cancellationToken)
        {
            // Validace je již hotová - proběhla v ValidationBehavior pipeline
            // request.RaceDto je garantovaně validní pokud se dostaneme sem

            _logger.LogInformation(
                "Updating Race with Id: {RaceId}, Name: {RaceName}, Category: {RaceCategory}",
                request.RaceDto.RaceId,
                request.RaceDto.RaceName,
                request.RaceDto.RaceCategory);

            try
            {
                // 1. Kontrola existence entity v databázi
                var existingRace = await _repository.GetByIdAsync(request.RaceDto.RaceId, cancellationToken);

                if (existingRace == null)
                {
                    _logger.LogWarning(
                        "Race with Id: {RaceId} not found for update",
                        request.RaceDto.RaceId);
                    throw new KeyNotFoundException($"Race with Id {request.RaceDto.RaceId} not found.");
                }

                // 2. Logování změn (volitelné - pro audit trail)
                //_logger.LogDebug(
                //    "Updating Race - Old name: {OldName}, New name: {NewName}", 
                //    existingRace.RaceName, 
                //    request.RaceDto.RaceName);

                // 3. Mapování DTO na Domain entitu
                var domain = _mapper.Map<Race>(request.RaceDto);

                // 4. Aktualizace přes repository
                await _repository.UpdateAsync(domain, cancellationToken);

                // 5. Úspěšné logování
                _logger.LogInformation(
                    "Race successfully updated - Id: {RaceId}, Name: {RaceName}, Category: {RaceCategory}",
                    domain.RaceId,
                    domain.RaceName,
                    domain.RaceCategory);

                return domain.RaceId;
            }
            catch (KeyNotFoundException)
            {
                // Re-throw známé výjimky bez logování (už zalogováno výše)
                throw;
            }
            catch (Exception ex)
            {
                // Logování neočekávaných chyb
                _logger.LogError(
                    ex,
                    "Error updating Race with Id: {RaceId}, Name: {RaceName}",
                    request.RaceDto.RaceId,
                    request.RaceDto.RaceName);
                throw; // Re-throw pro vyšší vrstvy (např. API middleware)
            }
        }
    }
}

