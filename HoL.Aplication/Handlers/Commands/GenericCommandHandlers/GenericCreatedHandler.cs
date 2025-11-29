using HoL.Aplication.LogHelpers;
using HoL.Aplication.LogHelpers.LogInterfaces.ICommandLog;

namespace HoL.Aplication.Handlers.Commands.GenericCommandHandlers
{
    /// <summary>
    /// Generický MediatR command handler pro vytváření nových entit.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity, která je vytvářena (např. Race, Character)</typeparam>
    /// <typeparam name="TDto">Typ DTO vstupních dat (např. RaceDto, CharacterDto)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující AddAsync metodu</typeparam>
    /// <remarks>
    /// <para>
    /// Handler provádí:
    /// <list type="number">
    /// <item><description>Validaci vstupních dat (automaticky přes ValidationBehavior)</description></item>
    /// <item><description>Mapování DTO na domain entitu</description></item>
    /// <item><description>Uložení entity do repository</description></item>
    /// <item><description>Strukturované logování operace</description></item>
    /// <item><description>Error handling s logováním výjimek</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Logování je implementováno pomocí <see cref="ILogCreated"/>, který zajišťuje:
    /// <list type="bullet">
    /// <item>Zalogování zahájení vytváření s parametry entity</item>
    /// <item>Zalogování úspěšného vytvoření s ID a názvem</item>
    /// <item>Zalogování chyb s podrobnostmi výjimky</item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Implementace pro Race entitu:
    /// <code>
    /// public class CreateRaceHandler 
    ///     : GenericCreatedHandler&lt;Race, RaceDto, IRaceRepository&gt;,
    ///       IRequestHandler&lt;CreateRaceCommand, int&gt;
    /// {
    ///     public CreateRaceHandler(
    ///         IRaceRepository repository,
    ///         IMapper mapper,
    ///         ILogger&lt;CreateRaceHandler&gt; logger)
    ///         : base(repository, mapper, logger, (repo, entity, ct) => repo.AddAsync(entity, ct))
    ///     {
    ///     }
    ///     
    ///     public async Task&lt;int&gt; Handle(CreateRaceCommand request, CancellationToken cancellationToken)
    ///     {
    ///         return await HandleCreate(request.RaceDto, cancellationToken, nameof(CreateRaceHandler));
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class GenericCreatedHandler<TEntity, TDto, TRepository>
        where TEntity : class
        where TDto : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Func<TRepository, TEntity, CancellationToken, Task<TEntity>> _addAsyncFunc;

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
        /// Metadata entita (RaceId, CharacterId, atd.) se naplní po uložení do databáze.
        /// </para>
        /// </remarks>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k chybě při mapování nebo uložení</exception>
        protected async Task<TDto> HandleCreate(
            TDto dto,
            CancellationToken cancellationToken,
            string handlerName)
        {
            ILogCreated logHelper = new LogHelpers<TEntity>(_logger, handlerName);


            try
            {
                logHelper.LogCreatingEntity();

                cancellationToken.ThrowIfCancellationRequested();

                // Mapování DTO na domain entitu
                var entity = _mapper.Map<TEntity>(dto);

                // Uložení entity do repository
                var newEntity = await _addAsyncFunc(_repository, entity, cancellationToken);

                logHelper.LogEntityCreatedSuccessfully();

                return _mapper.Map<TDto>(newEntity);
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
