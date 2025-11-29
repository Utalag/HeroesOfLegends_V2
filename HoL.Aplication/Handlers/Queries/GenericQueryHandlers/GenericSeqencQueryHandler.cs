using HoL.Aplication.LogHelpers;
using HoL.Aplication.LogHelpers.LogInterfaces.IQueriesLog;
using HoL.Domain.Enums;

namespace HoL.Aplication.Handlers.Queries.GenericQueryes
{
    /// <summary>
    /// Generický MediatR query handler pro sekvenční (stránkované) získání entit s možností řazení.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity (např. Race, Character)</typeparam>
    /// <typeparam name="TDto">Typ DTO pro výstup (např. RaceDto, CharacterDto)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující GetBySeqencAsync metodu</typeparam>
    /// <remarks>
    /// <para>
    /// Handler provádí:
    /// <list type="number">
    /// <item><description>Validaci vstupních parametrů (stránka, velikost, řazení)</description></item>
    /// <item><description>Sekvenční načtení entit z repository s podporou stránkování a řazení</description></item>
    /// <item><description>Mapování na DTO kolekci</description></item>
    /// <item><description>Logování operací a výsledků</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// <strong>Parametry stránkování:</strong>
    /// <list type="bullet">
    /// <item><term>page</term><description>Číslo stránky (0-based nebo 1-based dle implementace repository)</description></item>
    /// <item><term>size</term><description>Počet záznamů na stránku</description></item>
    /// <item><term>sortBy</term><description>Pole pro řazení (nullable - pokud null, použije se výchozí řazení)</description></item>
    /// <item><term>sortDir</term><description>Směr řazení (ascending/descending)</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Pokud nejsou nalezeny žádné entity, vrací <see cref="Enumerable.Empty{TDto}"/>.
    /// Handler automaticky respektuje <see cref="CancellationToken"/>.
    /// </para>
    /// </remarks>
    /// <example>
    /// Použití pro Race entity:
    /// <code>
    /// public class GetRaceSeqencQueryHandler 
    ///     : GenericSeqencQueryHandler&lt;Race, RaceDto, IRaceRepository&gt;,
    ///       IRequestHandler&lt;GetRaceSeqencQuery, IEnumerable&lt;RaceDto&gt;&gt;
    /// {
    ///     public GetRaceSeqencQueryHandler(
    ///         IRaceRepository repository, 
    ///         IMapper mapper, 
    ///         ILogger&lt;GetRaceSeqencQueryHandler&gt; logger)
    ///         : base(repository, mapper, logger, (page, size, sortBy, sortDir, ct) 
    ///             => repository.GetBySeqencAsync(page, size, sortBy, sortDir, ct))
    ///     {
    ///     }
    ///     
    ///     public async Task&lt;IEnumerable&lt;RaceDto&gt;&gt; Handle(GetRaceSeqencQuery request, CancellationToken cancellationToken)
    ///     {
    ///         return await HandleSeqencData(request.page, request.size, request.SortBy, request.SortDir, 
    ///             cancellationToken, nameof(GetRaceSeqencQueryHandler));
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class GenericSeqencQueryHandler<TEntity, TDto, TRepository>
        where TEntity : class
        where TDto : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Func<TRepository, int, int, string?, SortDirection, CancellationToken, Task<IEnumerable<TEntity>>> _getSequenceDataFunc;

        /// <summary>
        /// Inicializuje novou instanci generického handleru pro sekvenční data.
        /// </summary>
        /// <param name="repository">Repository implementující GetBySeqencAsync metodu</param>
        /// <param name="mapper">AutoMapper instance pro mapování entity na DTO</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <param name="getSequenceDataFunc">
        /// Delegát pro volání GetBySeqencAsync na konkrétním repository.
        /// Podpis: (page, size, sortBy, sortDir, cancellationToken) -> Task&lt;IEnumerable&lt;TEntity&gt;&gt;
        /// </param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
        protected GenericSeqencQueryHandler(TRepository repository,
                                     IMapper mapper,
                                     ILogger logger,
                                     Func<TRepository, int, int, string?, SortDirection, CancellationToken, Task<IEnumerable<TEntity>>> getSequenceDataFunc)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getSequenceDataFunc = getSequenceDataFunc ?? throw new ArgumentNullException(nameof(getSequenceDataFunc));
        }

        /// <summary>
        /// Zpracuje query pro sekvenční (stránkované) získání entit s podporou řazení.
        /// </summary>
        /// <param name="page">Číslo stránky pro stránkování</param>
        /// <param name="size">Počet záznamů na stránku</param>
        /// <param name="sortBy">Pole pro řazení (nullable - pokud null, použije se výchozí řazení)</param>
        /// <param name="sortDir">Směr řazení (ascending/descending)</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <param name="handlerName">Název handleru pro logování (např. "GetRaceSeqencQueryHandler")</param>
        /// <returns>
        /// Kolekce DTO pro nalezené entity na dané stránce. Prázdná kolekce pokud žádné entity nejsou nalezeny.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Metoda nikdy nevrací null - v případě prázdného výsledku vrací <see cref="Enumerable.Empty{TDto}"/>.
        /// </para>
        /// <para>
        /// Log messages:
        /// <list type="bullet">
        /// <item><description>Info: Zahájení načítání stránky</description></item>
        /// <item><description>Info: Počet vrácených entit</description></item>
        /// <item><description>Warning: Žádné entity nebyly nalezeny</description></item>
        /// <item><description>Warning: Operace byla zrušena</description></item>
        /// <item><description>Error: Chyba při zpracování dotazu</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k neočekávané chybě při načítání dat</exception>
        protected async Task<IEnumerable<TDto>> HandleSeqencData(
                                                                 int page,
                                                                 int size,
                                                                 string? sortBy,
                                                                 SortDirection sortDir,
                                                                 CancellationToken cancellationToken,
                                                                 string handlerName)
        {
            ILogSequence logHelper = new LogHelpers<TEntity>(_logger, handlerName);


            try
            {
                logHelper.LogSequenceQueryStart(page, size, sortBy, sortDir.ToString());

                var entities = await _getSequenceDataFunc(_repository, page, size, sortBy, sortDir, cancellationToken);

                if (entities == null || !entities.Any())
                {
                    logHelper.LogNoEntitiesFound();
                    return Enumerable.Empty<TDto>();
                }

                var entityList = entities.ToList();
                var dtoList = _mapper.Map<IEnumerable<TDto>>(entityList);
                var dtoCount = dtoList.Count();

                logHelper.LogSequenceQueryCompleted(dtoCount);
                return dtoList;
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
        }
    }
}
