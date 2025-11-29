using HoL.Aplication.LogHelpers;
using MediatR;

namespace HoL.Aplication.Handlers.Queries.GenericQueryes
{
    /// <summary>
    /// Generický MediatR query handler pro získání všech entit.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity (např. Race, Character)</typeparam>
    /// <typeparam name="TDto">Typ DTO pro výstup (např. RaceDto, CharacterDto)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující ListAsync metodu</typeparam>
    /// <remarks>
    /// <para>
    /// Handler provádí:
    /// <list type="number">
    /// <item><description>Načtení všech entit z repository</description></item>
    /// <item><description>Mapování na DTO</description></item>
    /// <item><description>Logování operací a výsledků</description></item>
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
    /// public class GetRaceAllQueryHandler 
    ///     : GenericAllQueryHandler&lt;Race, RaceDto, IRaceRepository&gt;,
    ///       IRequestHandler&lt;GetRaceAllQuery, IEnumerable&lt;RaceDto&gt;&gt;
    /// {
    ///     public GetRaceAllQueryHandler(
    ///         IRaceRepository repository, 
    ///         IMapper mapper, 
    ///         ILogger&lt;GetRaceAllQueryHandler&gt; logger)
    ///         : base(repository, mapper, logger, (repo, ct) => repo.ListAsync(ct))
    ///     {
    ///     }
    ///     
    ///     public async Task&lt;IEnumerable&lt;RaceDto&gt;&gt; Handle(GetRaceAllQuery request, CancellationToken cancellationToken)
    ///     {
    ///         return await HandleGetAll(cancellationToken, nameof(GetRaceAllQueryHandler));
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class GenericAllQueryHandler<TEntity, TDto, TRepository>
        where TEntity : class
        where TDto : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Func<TRepository, CancellationToken, Task<IEnumerable<TEntity>>> _getAllFunc;

        /// <summary>
        /// Inicializuje novou instanci generického handleru.
        /// </summary>
        /// <param name="repository">Repository implementující ListAsync metodu</param>
        /// <param name="mapper">AutoMapper instance pro mapování entity na DTO</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <param name="getAllFunc">Delegát pro volání ListAsync na konkrétním repository</param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
        protected GenericAllQueryHandler(TRepository repository,
                                         IMapper mapper,
                                         ILogger logger,
                                         Func<TRepository, CancellationToken, Task<IEnumerable<TEntity>>> getAllFunc)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getAllFunc = getAllFunc ?? throw new ArgumentNullException(nameof(getAllFunc));
        }

        /// <summary>
        /// Zpracuje query pro získání všech entit.
        /// </summary>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <param name="handlerName">Název handleru pro logování (např. "GetRaceAllQueryHandler")</param>
        /// <returns>
        /// Kolekce DTO pro všechny entity. Prázdná kolekce pokud žádné entity nebyly nalezeny.
        /// </returns>
        /// <remarks>
        /// Metoda nikdy nevrací null - v případě prázdného výsledku vrací <see cref="Enumerable.Empty{TDto}"/>.
        /// </remarks>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k neočekávané chybě při načítání dat</exception>
        protected async Task<IEnumerable<TDto>> HandleGetAll(CancellationToken cancellationToken, string handlerName)
        {
            ILogMultipleIds logHelper = new LogHelpers<TEntity>(_logger, handlerName);

            try
            {
                var entities = await _getAllFunc(_repository, cancellationToken);
                
                if (!entities.Any())
                {
                    logHelper.LogNoEntitiesFound();
                    return Enumerable.Empty<TDto>();
                }

                var entityList = entities.ToList();
                var dtoList = _mapper.Map<IEnumerable<TDto>>(entityList);
                var dtoListCount = dtoList.Count();

                logHelper.LogQueryCompleted(entityList.Count, dtoListCount);
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
