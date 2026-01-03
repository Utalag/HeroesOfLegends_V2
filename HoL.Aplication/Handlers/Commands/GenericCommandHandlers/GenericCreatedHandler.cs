using HoL.Domain.LogMessages;

namespace HoL.Aplication.Handlers.Commands.GenericCommandHandlers
{
    /// <summary>
    /// Generický MediatR command handler pro vytváření nových entit.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity, která je vytvářena (např. Race, Character)</typeparam>
    /// <typeparam name="TDto">Typ DTO vstupních dat (např. RaceDto, CharacterDto)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující AddAsync metodu</typeparam>
    public abstract class GenericCreatedHandler<TEntity, TDto, TRepository>
        where TEntity : class
        where TDto : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Func<TRepository, TEntity, CancellationToken, Task<TEntity>> _addAsyncFunc;
        private readonly string _entityName;

        /// <summary>
        /// Inicializuje novou instanci generického handleru pro vytváření entit.
        /// </summary>
        /// <param name="repository">Repository implementující AddAsync metodu</param>
        /// <param name="mapper">AutoMapper instance pro mapování DTO na entity</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <param name="addAsyncFunc">Delegát pro volání AddAsync na konkrétním repository</param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
        protected GenericCreatedHandler(
            TRepository repository,
            IMapper mapper,
            ILogger logger,
            Func<TRepository, TEntity, CancellationToken, Task<TEntity>> addAsyncFunc)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _addAsyncFunc = addAsyncFunc ?? throw new ArgumentNullException(nameof(addAsyncFunc));
            _entityName = typeof(TEntity).Name;
        }

        /// <summary>
        /// Zpracuje command pro vytvoření nové entity.
        /// </summary>
        /// <param name="dto">DTO obsahující data pro novou entitu (již validovaná)</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <param name="handlerName">Název konkrétního handleru pro logování (např. "CreateRaceHandler")</param>
        /// <returns>
        /// ID nově vytvořené entity (typicky generované databází).
        /// </returns>
        /// <remarks>
        /// <para>
        /// Proces:
        /// <list type="number">
        /// <item><description>Loguje zahájení vytváření</description></item>
        /// <item><description>Mapuje DTO na domain entitu</description></item>
        /// <item><description>Uloží entitu přes repository</description></item>
        /// <item><description>Loguje úspěšné vytvoření s ID</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Metadata entita (Id, CharacterId, atd.) se naplní po uložení do databáze.
        /// </para>
        /// </remarks>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k chybě při mapování nebo uložení</exception>
        protected async Task<TDto> HandleCreate(
            TDto dto,
            CancellationToken cancellationToken,
            string handlerName)
        {
            try
            {

                cancellationToken.ThrowIfCancellationRequested();

                // Mapování DTO na domain entitu
                var entity = _mapper.Map<TEntity>(dto);

                // Uložení entity do repository
                var newEntity = await _addAsyncFunc(_repository, entity, cancellationToken);


                return _mapper.Map<TDto>(newEntity);
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
