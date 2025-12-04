using HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace;
using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.LogMessages;

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
            _logger.LogInformation(LogMessageTemplates.Deleting.DeletingEntityInfo(
                nameof(RemoveRaceHandler),
                request.RaceDto.GetType().Name,
                request.RaceDto.RaceId));

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var existingRace = await _repository.ExistsAsync(request.RaceDto.RaceId, cancellationToken);

                if (existingRace)
                {
                    _logger.LogInformation(LogMessageTemplates.Deleting.EntityNotFoundForDeletion(
                        nameof(RemoveRaceHandler),
                        request.RaceDto.GetType().Name,
                        request.RaceDto.RaceId));

                    return true;
                }
                // removed
                await _repository.DeleteAsync(request.RaceDto.RaceId, cancellationToken);
                _logger.LogInformation(LogMessageTemplates.Deleting.EntityDeletedSuccessfully(
                    nameof(RemoveRaceHandler),
                        request.RaceDto.GetType().Name,
                        request.RaceDto.RaceId));
                return true;

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
