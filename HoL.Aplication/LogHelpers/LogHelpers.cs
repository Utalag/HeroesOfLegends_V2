using HoL.Aplication.LogHelpers.LogInterfaces;
using HoL.Aplication.LogHelpers.LogInterfaces.ICommandLog;
using HoL.Aplication.LogHelpers.LogInterfaces.IQueriesLog;

namespace HoL.Aplication.LogHelpers
{
    /// <summary>
    /// Generický helper pro strukturované logování v MediatR handlerech.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity pro kterou se loguje (např. Race, Character)</typeparam>
    /// <remarks>
    /// <para>
    /// Poskytuje standardizované log message pro běžné operace:
    /// <list type="bullet">
    /// <item>Získávání entit podle ID</item>
    /// <item>Získávání entit podle kolekce ID</item>
    /// <item>Stránkování a sekvenční načítání</item>
    /// <item>Chyby a výjimky</item>
    /// <item>Operace zrušení (cancellation)</item>
    /// </list>
    /// </para>
    /// <para>
    /// Všechny metody používají strukturované logování s placeholdery pro snadné vyhledávání.
    /// Handler name je nastaven v konstruktoru pro konzistenci napříč všemi log messages.
    /// </para>
    /// </remarks>
    /// <example>
    /// Použití v handleru:
    /// <code>
    /// var logHelper = new LogHelpers&lt;Race&gt;(_logger, "GetRaceByIdQuery");
    /// logHelper.LogEntityNotFound(id);
    /// </code>
    /// </example>
    public class LogHelpers<TEntity> : 
        ILogMultipleIds, 
        ICommonLog, 
        ILogExceptions, 
        ILogSingleId, 
        ILogSequence, 
        ILogDeleted,
        ILogUpdated,
        ILogCreated
        where TEntity : class
    {
        private readonly ILogger _logger;
        private readonly string _entityName;
        private readonly string _handlerName;

        /// <summary>
        /// Inicializuje novou instanci <see cref="LogHelpers{TEntity}"/>.
        /// </summary>
        /// <param name="logger">Logger instance pro zápis logů</param>
        /// <param name="handlerName">Název handleru (např. "GetRaceByIdQuery") pro konzistentní logování</param>
        /// <exception cref="ArgumentNullException">Pokud logger nebo handlerName je null</exception>
        public LogHelpers(ILogger logger, string handlerName)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handlerName = handlerName ?? throw new ArgumentNullException(nameof(handlerName));
            _entityName = typeof(TEntity).Name;
        }

        #region GetById Logs

        /// <summary>
        /// Loguje nevalidní ID (menší než 1).
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> Invalid ID {Id} (must be > 0). Returning null."</c>
        /// </remarks>
        /// <param name="id">Nevalidní ID</param>
        public void LogInvalidId(object id)
        {
            _logger.LogWarning("{Handler} -> Invalid ID {Id} (must be > 0). Returning null.",
                _handlerName, id.ToString());
        }

        /// <summary>
        /// Loguje zahájení načítání entity podle ID.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> Retrieving {Entity} with ID {Id}."</c>
        /// </remarks>
        /// <param name="id">ID entity k načtení</param>
        public void LogRetrievingEntity(object id)
        {
            _logger.LogInformation("{Handler} -> Retrieving {Entity} with ID {Id}.",
                _handlerName, _entityName, id.ToString());
        }

        /// <summary>
        /// Loguje, že entita nebyla nalezena.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> {Entity} with ID {Id} not found."</c>
        /// </remarks>
        /// <param name="id">ID nenalezené entity</param>
        public void LogEntityNotFound(object id)
        {
            _logger.LogWarning("{Handler} -> {Entity} with ID {Id} not found.",
                _handlerName, _entityName, id.ToString());
        }

        /// <summary>
        /// Loguje úspěšné načtení entity.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> {Entity} with ID {Id} successfully retrieved."</c>
        /// </remarks>
        /// <param name="id">ID načtené entity</param>
        public void LogEntityRetrievedSuccessfully(object id)
        {
            _logger.LogInformation("{Handler} -> {Entity} with ID {Id} successfully retrieved.",
                _handlerName, _entityName, id.ToString());
        }

        #endregion

        #region GetByIds Logs

        /// <summary>
        /// Loguje, že kolekce ID je null.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> Ids collection is null. Returning empty result."</c>
        /// </remarks>
        public void LogNullIdsCollection()
        {
            _logger.LogWarning("{Handler} -> Ids collection is null. Returning empty result.",
                _handlerName);
        }

        /// <summary>
        /// Loguje, že kolekce neobsahuje validní ID.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> No valid IDs (>0) provided. Returning empty result."</c>
        /// </remarks>
        public void LogNoValidIds()
        {
            _logger.LogWarning("{Handler} -> No valid IDs (>0) provided. Returning empty result.",
                _handlerName);
        }

        /// <summary>
        /// Loguje zpracování kolekce ID s normalizací.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> Processing {RawCount} raw IDs, {NormalizedCount} normalized IDs: {Ids}"</c>
        /// </remarks>
        /// <param name="rawCount">Počet surových ID</param>
        /// <param name="normalizedCount">Počet normalizovaných ID</param>
        /// <param name="ids">Normalizovaná kolekce ID</param>
        public void LogProcessingIds(int rawCount, int normalizedCount, IEnumerable<object> ids)
        {
            _logger.LogInformation(
                "{Handler} -> Processing {RawCount} raw IDs, {NormalizedCount} normalized IDs: {Ids}",
                _handlerName, rawCount, normalizedCount, string.Join(", ", ids));
        }

        /// <summary>
        /// Loguje dokončení query s počtem nalezených entit.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> Query completed: requestedCount {Requested}, returnedCount {Returned}."</c>
        /// </remarks>
        /// <param name="requestedCount">Počet požadovaných ID</param>
        /// <param name="returnedCount">Počet vrácených entit</param>
        public void LogQueryCompleted(int requestedCount, int returnedCount)
        {
            _logger.LogInformation(
                "{Handler} -> Query completed: requestedCount {Requested}, returnedCount {Returned}.",
                _handlerName, requestedCount, returnedCount);
        }

        /// <summary>
        /// Loguje, že žádné entity nebyly nalezeny.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> Query completed: no {Entity} found."</c>
        /// </remarks>
        public void LogNoEntitiesFound()
        {
            _logger.LogInformation("{Handler} -> Query completed: no {Entity} found.",
                _handlerName, _entityName);
        }

        #endregion

        #region Sequence/Pagination Logs

        /// <summary>
        /// Loguje zahájení stránkovaného dotazu.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> Retrieving {Entity} page {Page}, size {Size}, sortBy: {SortBy}, direction: {SortDir}"</c>
        /// </remarks>
        /// <param name="page">Číslo stránky</param>
        /// <param name="size">Velikost stránky</param>
        /// <param name="sortBy">Pole pro řazení (nullable)</param>
        /// <param name="sortDir">Směr řazení</param>
        public void LogSequenceQueryStart(int page, int size, string? sortBy, string sortDir)
        {
            _logger.LogInformation(
                "{Handler} -> Retrieving {Entity} page {Page}, size {Size}, sortBy: {SortBy}, direction: {SortDir}",
                _handlerName, _entityName, page, size, sortBy ?? "default", sortDir);
        }

        /// <summary>
        /// Loguje úspěšné dokončení stránkovaného dotazu.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> Successfully retrieved {Count} {Entity}(s)."</c>
        /// </remarks>
        /// <param name="count">Počet vrácených entit</param>
        public void LogSequenceQueryCompleted(int count)
        {
            _logger.LogInformation("{Handler} -> Successfully retrieved {Count} {Entity}(s).",
                _handlerName, count, _entityName);
        }

        #endregion

        #region Error & Exception Logs

        /// <summary>
        /// Loguje zrušení operace (cancellation).
        /// </summary>
        /// <remarks>
        /// Log message (s ID): <c>"{Handler} -> Operation canceled for {Entity} with ID {Id}."</c>
        /// <br/>
        /// Log message (bez ID): <c>"{Handler} -> Operation canceled."</c>
        /// </remarks>
        /// <param name="id">ID entity (nullable pro batch operace)</param>
        public void LogOperationCanceled(object? id = null)
        {
            if (id != null)
            {
                _logger.LogWarning("{Handler} -> Operation canceled for {Entity} with ID {Id}.",
                    _handlerName, _entityName, id.ToString());
            }
            else
            {
                _logger.LogWarning("{Handler} -> Operation canceled.",
                    _handlerName);
            }
        }

        /// <summary>
        /// Loguje neočekávanou chybu při zpracování.
        /// </summary>
        /// <remarks>
        /// Log message (s ID): <c>"{Handler} -> Unexpected error retrieving {Entity} with ID {Id}."</c>
        /// <br/>
        /// Log message (bez ID): <c>"{Handler} -> Unexpected error retrieving {Entity}."</c>
        /// </remarks>
        /// <param name="exception">Zachycená výjimka</param>
        /// <param name="id">ID entity (nullable pro batch operace)</param>
        public void LogUnexpectedError(Exception exception, object? id = null)
        {
            if (id != null)
            {
                _logger.LogError(exception, "{Handler} -> Unexpected error retrieving {Entity} with ID {Id}.",
                    _handlerName, _entityName, id.ToString());
            }
            else
            {
                _logger.LogError(exception, "{Handler} -> Unexpected error retrieving {Entity}.",
                    _handlerName, _entityName);
            }
        }

        /// <summary>
        /// Loguje validační chybu.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> Validation failed: {ValidationMessage}"</c>
        /// </remarks>
        /// <param name="validationMessage">Zpráva validační chyby</param>
        public void LogValidationError(string validationMessage)
        {
            _logger.LogWarning("{Handler} -> Validation failed: {ValidationMessage}",
                _handlerName, validationMessage);
        }

        #endregion

        #region Generic Logs

        /// <summary>
        /// Loguje obecnou informační zprávu.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> {Message}"</c>
        /// </remarks>
        /// <param name="message">Zpráva k zalogování</param>
        public void LogInfo(string message)
        {
            _logger.LogInformation("{Handler} -> {Message}",
                _handlerName, message);
        }

        /// <summary>
        /// Loguje obecné varování.
        /// </summary>
        /// <remarks>
        /// Log message: <c>"{Handler} -> {Message}"</c>
        /// </remarks>
        /// <param name="message">Varovná zpráva</param>
        public void LogWarning(string message)
        {
            _logger.LogWarning("{Handler} -> {Message}",
                _handlerName, message);
        }

        #endregion

        public void LogNoQueryReqested()
        {
            _logger.LogInformation("{Handler} -> No query requestedCount. Returning empty result.",
                _handlerName);
        }

        #region Create
        public void LogCreatingEntity()
        {
            _logger.LogInformation("{Handler} -> Creating new {Entity}.",
                _handlerName, _entityName);
        }

        public void LogEntityCreatedSuccessfully()
        {
            _logger.LogInformation("{Handler} -> {Entity} created successfully.",
                _handlerName,_entityName );
        }

        #endregion

        #region Update

        public void LogUpdatingEntityInfo(object id)
        {
            _logger.LogInformation("{Handler} -> Updating {Entity} with ID {Id}.",
                _handlerName, _entityName, id.ToString());
        }

        public void LogEntityUpdatedSuccessfully(object id)
        {
            _logger.LogInformation("{Handler} -> {Entity} with ID {Id} updated successfully.",
                _handlerName, _entityName, id.ToString());
        }
        #endregion

        #region Delete
        public void LogDeletingEntityInfo(object id)
        {
            _logger.LogInformation("{Handler} -> Deleting {Entity} with ID {Id}.",
                _handlerName, _entityName, id.ToString());
        }
        public void LogEntityDeletedSuccessfully(object id)
        {
            _logger.LogInformation("{Handler} -> {Entity} with ID {Id} deleted successfully.",
                _handlerName, _entityName, id.ToString());
        }
        #endregion
    }


    


    


  







}
