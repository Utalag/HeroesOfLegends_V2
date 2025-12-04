using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.Entities;
using HoL.Domain.LogMessages;

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
            _logger.LogInformation(LogMessageTemplates.Updating.UpdatingEntityWithId(
                nameof(UpdatedRaceHandler),
                request.RaceDto.GetType().Name,
                request.RaceDto.RaceId));

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var existingRace = await _repository.ExistsAsync(request.RaceDto.RaceId, cancellationToken);

                if (existingRace)
                {
                    _logger.LogInformation(LogMessageTemplates.Existence.EntityDoesNotExist(
                        nameof(UpdatedRaceHandler),
                        request.RaceDto.GetType().Name,
                        request.RaceDto.RaceId));
                    return 0;
                }

                var domain = _mapper.Map<Race>(request.RaceDto);

                await _repository.UpdateAsync(domain, cancellationToken);
                _logger.LogInformation(
                    LogMessageTemplates.Updating.EntityUpdatedSuccessfullyWithId(
                        nameof(UpdatedRaceHandler),
                        request.RaceDto.GetType().Name,
                        request.RaceDto.RaceId));

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

