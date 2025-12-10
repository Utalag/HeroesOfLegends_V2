using HoL.Domain.LogMessages;

namespace HoL.Aplication.Handlers.Commands.GenericCommandHandlers
{
    /// <summary>
    /// Generický MediatR command handler pro smazání existujících entit.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity, která je mazána (např. Race, Character)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující DeleteAsync a ExistsAsync metody</typeparam>
    public abstract class GenericRemovedHandler<TEntity, TRepository>
        where TEntity : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly ILogger _logger;
        private readonly Func<TRepository, object, CancellationToken, Task<bool>> _existsAsyncFunc;
        private readonly Func<TRepository, object, CancellationToken, Task> _deleteAsyncFunc;
        private readonly string _entityName;

        protected GenericRemovedHandler(
            TRepository repository,
            ILogger logger,
            Func<TRepository, object, CancellationToken, Task<bool>> existsAsyncFunc,
            Func<TRepository, object, CancellationToken, Task> deleteAsyncFunc)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _existsAsyncFunc = existsAsyncFunc ?? throw new ArgumentNullException(nameof(existsAsyncFunc));
            _deleteAsyncFunc = deleteAsyncFunc ?? throw new ArgumentNullException(nameof(deleteAsyncFunc));
            _entityName = typeof(TEntity).Name;
        }

        protected async Task<bool> HandleDelete(
            int entityId,
            CancellationToken cancellationToken,
            string handlerName)
        {
            try
            {
                
                cancellationToken.ThrowIfCancellationRequested();

                bool exists = await _existsAsyncFunc(_repository, entityId, cancellationToken);

                if (!exists)
                {
                }

                await _deleteAsyncFunc(_repository, entityId, cancellationToken);

                return true;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
               throw;
            }
        }
    }
}
