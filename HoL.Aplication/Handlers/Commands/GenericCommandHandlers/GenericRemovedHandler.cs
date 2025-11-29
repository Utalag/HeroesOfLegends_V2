using HoL.Aplication.LogHelpers;
using HoL.Aplication.LogHelpers.LogInterfaces.ICommandLog;

namespace HoL.Aplication.Handlers.Commands.GenericCommandHandlers
{
    /// <summary>
    /// Generický MediatR command handler pro smazání existujících entit.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity, která je mazána (např. Race, Character)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující DeleteAsync a ExistsAsync metody</typeparam>
    /// <remarks>
    /// <para>
    /// Handler provádí:
    /// <list type="number">
    /// <item><description>Ověření existence entity v databázi</description></item>
    /// <item><description>Smazání entity z repository</description></item>
    /// <item><description>Strukturované logování operace</description></item>
    /// <item><description>Error handling s logováním výjimek</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Logování je implementováno pomocí <see cref="LogDeletedHelpers{TEntity}"/>, který zajišťuje:
    /// <list type="bullet">
    /// <item>Zalogování zahájení smazání s ID entity</item>
    /// <item>Zalogování úspěšného smazání</item>
    /// <item>Zalogování chyb včetně nenalezení entity</item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Implementace pro Race entitu:
    /// <code>
    /// public class DeleteRaceHandler 
    ///     : GenericRemovedHandler&lt;Race, IRaceRepository&gt;,
    ///       IRequestHandler&lt;DeleteRaceCommand, bool&gt;
    /// {
    ///     public DeleteRaceHandler(
    ///         IRaceRepository repository,
    ///         ILogger&lt;DeleteRaceHandler&gt; logger)
    ///         : base(repository, logger, (repo, id, ct) => repo.ExistsAsync(id, ct),
    ///             (repo, id, ct) => repo.DeleteAsync(id, ct))
    ///     {
    ///     }
    ///     
    ///     public async Task&lt;bool&gt; Handle(DeleteRaceCommand request, CancellationToken cancellationToken)
    ///     {
    ///         return await HandleDelete(request.RaceId, request.RaceName, cancellationToken, nameof(DeleteRaceHandler));
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class GenericRemovedHandler<TEntity, TRepository>
        where TEntity : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly ILogger _logger;
        private readonly Func<TRepository, object, CancellationToken, Task<bool>> _existsAsyncFunc;
        private readonly Func<TRepository, object, CancellationToken, Task> _deleteAsyncFunc;

        /// <summary>
        /// Inicializuje novou instanci generického handleru pro smazání entit.
        /// </summary>
        /// <param name="repository">Repository implementující DeleteAsync a ExistsAsync metody</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <param name="existsAsyncFunc">Delegát pro ověření existence entity</param>
        /// <param name="deleteAsyncFunc">Delegát pro smazání entity</param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
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
        }

        /// <summary>
        /// Zpracuje command pro smazání existující entity.
        /// </summary>
        /// <param name="entityId">ID entity k smazání</param>
        /// <param name="entityName">Název entity pro logování (volitelné)</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <param name="handlerName">Název konkrétního handleru pro logování (např. "DeleteRaceHandler")</param>
        /// <returns>
        /// True pokud smazání proběhlo úspěšně, jinak false.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Proces:
        /// <list type="number">
        /// <item><description>Loguje zahájení smazání</description></item>
        /// <item><description>Ověří existenci entity v databázi</description></item>
        /// <item><description>Smaže entitu přes repository</description></item>
        /// <item><description>Loguje úspěšné smazání</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Pokud entita neexistuje, vyvolá <see cref="KeyNotFoundException"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Pokud entita s daným ID neexistuje</exception>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k chybě při smazání</exception>
        protected async Task<bool> HandleDelete(
            int entityId,
            CancellationToken cancellationToken,
            string handlerName)
        {
            ILogDeleted logHelper = new LogHelpers<TEntity>(_logger, handlerName);

            try
            {
                logHelper.LogDeletingEntityInfo(entityId);

                cancellationToken.ThrowIfCancellationRequested();

                // Ověření existence entity
                bool exists = await _existsAsyncFunc(_repository, entityId, cancellationToken);

                if (!exists)
                {
                    logHelper.LogEntityNotFound(entityId);

                }

                // Smazání entity z repository
                await _deleteAsyncFunc(_repository, entityId, cancellationToken);

                logHelper.LogEntityDeletedSuccessfully(entityId);

                return true;
            }
            catch (OperationCanceledException)
            {
                logHelper.LogOperationCanceled(entityId);
                throw;
            }
            catch (Exception ex)
            {
                logHelper.LogUnexpectedError(ex, entityId);
                throw;
            }
        }
    }
}
