using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.Entities;

namespace HeroesOfLegends.Application.Handlers.Commands.RaceCommand.CreatedRace
{
    public class CreatedRaceHandler : IRequestHandler<CreatedRaceCommand, int>
    {
        private readonly IRaceRepository _repository;
        private readonly ILogger<CreatedRaceHandler> _logger;
        private readonly IMapper _mapper;

        public CreatedRaceHandler(
            IRaceRepository repository, 
            ILogger<CreatedRaceHandler> logger,
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<int> Handle(CreatedRaceCommand request, CancellationToken cancellationToken)
        {
            // ValidationBehavior už zkontroloval že request.RaceDto je validní
            // Pokud by validace selhala, tento kód by se NIKDY nespustil

            _logger.LogInformation("Creating new RaceDto: {RaceName}", request.RaceDto.RaceName);

            try
            {
                // Mapování pomocí AutoMapper - vše je definováno v Mapper profilu
                var domain = _mapper.Map<Race>(request.RaceDto);

                // Uložení přes repository
                await _repository.AddAsync(domain, cancellationToken);

                _logger.LogInformation(
                    "RaceDto created successfully - Id: {RaceId}, Name: {RaceName}, Category: {RaceCategory}",
                    domain.RaceId,
                    domain.RaceName,
                    domain.RaceCategory);

                return domain.RaceId;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating RaceDto: {RaceName}",
                    request.RaceDto.RaceName);
                throw;
            }
        }
    }
}