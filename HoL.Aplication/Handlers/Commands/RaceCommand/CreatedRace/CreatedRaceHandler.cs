using HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace;
using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.Entities;
using HoL.Domain.LogMessages;

namespace HeroesOfLegends.Application.Handlers.Commands.RaceCommand.CreatedRace
{
    public class CreatedRaceHandler : IRequestHandler<CreatedRaceCommand, int>
    {
        private readonly IRaceRepository _repository;
        private readonly ILogger<CreatedRaceHandler> _logger;
        private readonly IMapper _mapper;
        private readonly string source;

        public CreatedRaceHandler(
            IRaceRepository repository,
            ILogger<CreatedRaceHandler> logger,
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            source = this.GetType().Name;
        }


        public async Task<int> Handle(CreatedRaceCommand request, CancellationToken cancellationToken)
        {
            // ValidationBehavior už zkontroloval že request.RaceDto je validní
            // Pokud by validace selhala, tento kód by se NIKDY nespustil

            _logger.LogInformation(LogMessageTemplates.Creating.CreatingEntity(
                source,
                request.RaceDto.GetType().Name));

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Mapování pomocí AutoMapper - vše je definováno v Mapper profilu
                var domain = _mapper.Map<Race>(request.RaceDto);

                // Uložení přes repository
                await _repository.AddAsync(domain, cancellationToken);
                _logger.LogInformation(LogMessageTemplates.Creating.EntityCreatedSuccessfully(
                    source,
                    request.RaceDto.GetType().Name));

                return domain.RaceId;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning(
                    LogMessageTemplates.Exceptions.OperationCanceledWithId(
                        nameof(UpdatedRaceHandler),
                        request.RaceDto.GetType().Name,
                        request.RaceDto.RaceId));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    LogMessageTemplates.Exceptions.UnexpectedErrorWithId(
                        nameof(UpdatedRaceHandler),
                        request.RaceDto.GetType().Name,
                        request.RaceDto.RaceId,
                        ex));
                throw;
            }
        }
    }
}