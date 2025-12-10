using HoL.Domain.LogMessages;

namespace HoL.Aplication.Handlers.Queries.GenericQueryes
{
    /// <summary>
    /// Generický MediatR query handler pro získání entity podle ID.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity (např. Race, Character)</typeparam>
    /// <typeparam name="TDto">Typ DTO pro výstup (např. RaceDto, CharacterDto)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující GetByIdAsync</typeparam>
    /// <remarks>
    /// <para>
    /// Handler provádí:
    /// <list type="number">
    /// <item><description>Validace ID (musí být > 0)</description></item>
    /// <item><description>Načtení entity z repository</description></item>
    /// <item><description>Mapování na DTO</description></item>
    /// <item><description>Logování operací a výsledků</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Pokud entita není nalezena, vrací <c>null</c>.
    /// Handler automaticky respektuje <see cref="CancellationToken"/>
    /// </para>
    /// </remarks>
    /// <example>
    /// Použití pro Race entity:
    /// <code>
    /// public class GetRaceByIdQueryHandler 
    ///     : GenericIdQueryHandler&lt;Race, RaceDto, IRaceRepository&gt;,
    ///       IRequestHandler&lt;GetRaceByIdQuery, RaceDto?&gt;
    /// {
    ///     public GetRaceByIdQueryHandler(
    ///         IRaceRepository repository, 
    ///         IMapper mapper, 
    ///         ILogger&lt;GetRaceByIdQueryHandler&gt; logger)
    ///         : base(repository, mapper, logger, (repo, id, ct) => repo.GetByIdAsync(id, ct))
    ///     {
    ///     }
    ///     
    ///     public async Task&lt;RaceDto?&gt; Handle(GetRaceByIdQuery request, CancellationToken cancellationToken)
    ///     {
    ///         return await HandleGetById(request.Id, cancellationToken, nameof(GetRaceByIdQueryHandler));
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class GenericIdQueryHandler<TEntity, TDto, TRepository>
        where TEntity : class
        where TDto : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Func<TRepository, int, CancellationToken, Task<TEntity?>> _getByIdFunc;
        private readonly string _entityName;

        /// <summary>
        /// Inicializuje novou instanci generického handleru.
        /// </summary>
        /// <param name="repository">Repository implementující GetByIdAsync metodu</param>
        /// <param name="mapper">AutoMapper instance pro mapování entity na DTO</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <param name="getByIdFunc">Delegát pro volání GetByIdAsync na konkrétním repository</param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
        protected GenericIdQueryHandler(TRepository repository,
                                        IMapper mapper,
                                        ILogger logger,
                                        Func<TRepository, int, CancellationToken, Task<TEntity?>> getByIdFunc)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getByIdFunc = getByIdFunc ?? throw new ArgumentNullException(nameof(getByIdFunc));
            _entityName = typeof(TEntity).Name;
        }

        /// <summary>
        /// Zpracuje query pro získání entity podle ID.
        /// </summary>
        /// <param name="id">ID entity k načtení (musí být > 0)</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <param name="handlerName">Název handleru pro logování</param>
        /// <returns>
        /// DTO entity pokud nalezena, jinak <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Metoda automaticky validuje ID a loguje všechny operace.
        /// Pokud je ID menší než 1, okamžitě vrací <c>null</c> bez dotazu do databáze.
        /// </remarks>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k neočekávané chybě při načítání dat</exception>
        protected async Task<TDto?> HandleGetById(int id, CancellationToken cancellationToken, string handlerName)
        {
            if (id < 1)
            {
                
                return null;
            }

            

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var entity = await _getByIdFunc(_repository, id, cancellationToken);

                if (entity is null)
                {
                    
                    return null;
                }

                var dto = _mapper.Map<TDto>(entity);

                return dto;
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
