using HoL.Aplication.LogHelpers;

namespace HoL.Aplication.Handlers.Queries.GenericQueryes
{
    /// <summary>
    /// Generický MediatR query handler pro získání entit podle kolekce ID.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity (např. Race, Character)</typeparam>
    /// <typeparam name="TDto">Typ DTO pro výstup (např. RaceDto, CharacterDto)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující základní CRUD operace</typeparam>
    /// <remarks>
    /// <para>
    /// Handler provádí:
    /// <list type="number">
    /// <item><description>Validace a normalizace vstupní kolekce ID (odstranění duplikátů, filtrování nevalidních hodnot)</description></item>
    /// <item><description>Iterativní načtení entit z repository s podporou cancellation</description></item>
    /// <item><description>Mapování nalezených entit na DTO</description></item>
    /// <item><description>Logování průběhu operace a výsledků</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// <strong>Normalizace vstupů:</strong>
    /// <list type="bullet">
    /// <item>Odstranění ID menších nebo rovných 0</item>
    /// <item>Odstranění duplicitních ID</item>
    /// <item>Zachování pořadí prvního výskytu každého ID</item>
    /// </list>
    /// </para>
    /// <para>
    /// Handler loguje všechny operace včetně:
    /// - Počet surových vs. normalizovaných ID
    /// - Nenalezené entity (jako warning)
    /// - Finální počet vrácených výsledků
    /// </para>
    /// </remarks>
    /// <example>
    /// Použití pro Race entity:
    /// <code>
    /// public class GetRacesByIdsQueryHandler 
    ///     : GenericGetByIdsQueryHandler&lt;Race, RaceDto, IRaceRepository&gt;
    /// {
    ///     public GetRacesByIdsQueryHandler(
    ///         IRaceRepository repository, 
    ///         IMapper mapper, 
    ///         ILogger&lt;GetRacesByIdsQueryHandler&gt; logger)
    ///         : base(repository, mapper, logger, (repo, id, ct) => repo.GetByIdAsync(id, ct))
    ///     {
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class GenericIdsQueryHandler<TEntity, TDto, TRepository>
        where TEntity : class
        where TDto : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Func<TRepository, int, CancellationToken, Task<TEntity?>> _getByIdFunc;

        /// <summary>
        /// Inicializuje novou instanci generického handleru.
        /// </summary>
        /// <param name="repository">Repository implementující GetByIdAsync metodu</param>
        /// <param name="mapper">AutoMapper instance pro mapování entity na DTO</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <param name="getByIdFunc">Delegát pro volání GetByIdAsync na konkrétním repository</param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
        protected GenericIdsQueryHandler(
            TRepository repository,
            IMapper mapper,
            ILogger logger,
            Func<TRepository, int, CancellationToken, Task<TEntity?>> getByIdFunc)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getByIdFunc = getByIdFunc ?? throw new ArgumentNullException(nameof(getByIdFunc));
        }

        /// <summary>
        /// Zpracuje query pro získání entit podle kolekce ID.
        /// </summary>
        /// <param name="ids">Kolekce ID k načtení</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>
        /// Kolekce DTO pro nalezené entity. Prázdná kolekce pokud žádné entity nebyly nalezeny.
        /// </returns>
        /// <remarks>
        /// Metoda nikdy nevrací null - v případě prázdného výsledku vrací <see cref="Enumerable.Empty{TDto}"/>.
        /// </remarks>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k neočekávané chybě při načítání dat</exception>
        protected async Task<IEnumerable<TDto>> HandleGetByIds(
            IEnumerable<int>? ids,
            CancellationToken cancellationToken,
            string handlerName)
        {
            ILogMultipleIds logHelper = new LogHelpers<TEntity>(_logger, handlerName);

            if (ids is null)
            {
                logHelper.LogNullIdsCollection();
                return Enumerable.Empty<TDto>();
            }

            var normalizedIds = ids
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (!normalizedIds.Any())
            {
                logHelper.LogNoValidIds();
                return Enumerable.Empty<TDto>();
            }

            logHelper.LogProcessingIds(ids.Count(),
                                       normalizedIds.Count,
                                       normalizedIds);

            var foundEntities = new List<TEntity>(normalizedIds.Count);

            try
            {
                foreach (var id in normalizedIds)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var entity = await _getByIdFunc(_repository, id, cancellationToken);
                    if (entity is null)
                    {
                        logHelper.LogEntityNotFound(id);
                        continue;
                    }

                    foundEntities.Add(entity);
                }
            }
            catch (OperationCanceledException)
            {
                logHelper.LogOperationCanceled();
                throw;
            }
            catch (Exception ex)
            {
                logHelper.LogUnexpectedError(ex);
                throw;
            }

            if (foundEntities.Count == 0)
            {
                logHelper.LogNoEntitiesFound();
                return Enumerable.Empty<TDto>();
            }

            var dtoList = _mapper.Map<List<TDto>>(foundEntities);

            logHelper.LogQueryCompleted(normalizedIds.Count, dtoList.Count);

            return dtoList;
        }
    }
}
