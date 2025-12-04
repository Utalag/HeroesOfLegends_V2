using HoL.Domain.LogMessages;

namespace HoL.Aplication.Handlers.Commands.GenericCommandHandlers
{
    public abstract class GenericUpdatedHandler<TEntity, TDto, TRepository>
        where TEntity : class
        where TDto : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Func<TRepository, object, CancellationToken, Task<bool>> _existsAsyncFunc;
        private readonly Func<TRepository, TEntity, CancellationToken, Task<TEntity>> _updateAsyncFunc;
        private readonly string _entityName;

        protected GenericUpdatedHandler(
            TRepository repository,
            IMapper mapper,
            ILogger logger,
            Func<TRepository, object, CancellationToken, Task<bool>> existsAsyncFunc,
            Func<TRepository, TEntity, CancellationToken, Task<TEntity>> updateAsyncFunc)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _existsAsyncFunc = existsAsyncFunc ?? throw new ArgumentNullException(nameof(existsAsyncFunc));
            _updateAsyncFunc = updateAsyncFunc ?? throw new ArgumentNullException(nameof(updateAsyncFunc));
            _entityName = typeof(TEntity).Name;
        }

        protected async Task<TDto> HandleUpdate(
            TDto dto,
            int id,
            CancellationToken cancellationToken,
            string handlerName)
        {
            try
            {
                _logger.LogInformation(LogMessageTemplates.Updating.UpdatingEntityWithId(handlerName, _entityName, id));

                cancellationToken.ThrowIfCancellationRequested();

                bool exists = await _existsAsyncFunc(_repository, id, cancellationToken);

                if (!exists)
                {
                    _logger.LogWarning(LogMessageTemplates.GetById.EntityNotFound(handlerName, _entityName, id));
                    _logger.LogWarning(LogMessageTemplates.Generic.Warning(handlerName, "Return originalDto without update."));
                    return dto;
                }

                var entity = _mapper.Map<TEntity>(dto);
                var updatedEntity = await _updateAsyncFunc(_repository, entity, cancellationToken);

                _logger.LogInformation(LogMessageTemplates.Updating.EntityUpdatedSuccessfullyWithId(handlerName, _entityName, id));

                return _mapper.Map<TDto>(updatedEntity);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning(LogMessageTemplates.Exceptions.OperationCanceledWithId(handlerName, _entityName, id));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessageTemplates.Exceptions.UnexpectedErrorWithId(handlerName, _entityName, id, ex));
                throw;
            }
        }
    }
}
