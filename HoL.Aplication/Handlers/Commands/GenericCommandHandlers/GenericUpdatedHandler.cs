using HoL.Aplication.LogHelpers;
using HoL.Aplication.LogHelpers.LogInterfaces.ICommandLog;

namespace HoL.Aplication.Handlers.Commands.GenericCommandHandlers
{
    /// <summary>
    /// Generický MediatR command handler pro aktualizaci existujících entit.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity, která je aktualizována (např. Race, Character)</typeparam>
    /// <typeparam name="TDto">Typ DTO vstupních dat (např. RaceDto, CharacterDto)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující UpdateAsync a ExistsAsync metody</typeparam>
    /// <remarks>
    /// <para>
    /// Handler provádí:
    /// <list type="number">
    /// <item><description>Validaci vstupních dat (automaticky přes ValidationBehavior)</description></item>
    /// <item><description>Kontrolu existence entity v databázi</description></item>
    /// <item><description>Mapování DTO na domain entitu</description></item>
    /// <item><description>Aktualizaci entity v repository</description></item>
    /// <item><description>Strukturované logování operace</description></item>
    /// <item><description>Error handling s logováním výjimek</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Logování je implementováno pomocí <see cref="LogHelpers{TEntity}"/>, který zajišťuje:
    /// <list type="bullet">
    /// <item>Zalogování zahájení aktualizace s parametry entity</item>
    /// <item>Zalogování úspěšné aktualizace</item>
    /// <item>Zalogování chyb včetně nenalezení entity</item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Implementace pro Race entitu:
    /// <code>
    /// public class UpdateRaceHandler 
    ///     : GenericUpdatedHandler&lt;Race, RaceDto, IRaceRepository&gt;,
    ///       IRequestHandler&lt;UpdateRaceCommand, bool&gt;
    /// {
    ///     public UpdateRaceHandler(
    ///         IRaceRepository repository,
    ///         IMapper mapper,
    ///         ILogger&lt;UpdateRaceHandler&gt; logger)
    ///         : base(repository, mapper, logger, (repo, id, ct) => repo.ExistsAsync(id, ct),
    ///             (repo, entity, ct) => repo.UpdateAsync(entity, ct))
    ///     {
    ///     }
    ///     
    ///     public async Task&lt;bool&gt; Handle(UpdateRaceCommand request, CancellationToken cancellationToken)
    ///     {
    ///         return await HandleUpdate(request.RaceDto, cancellationToken, nameof(UpdateRaceHandler));
    ///     }
    /// }
    /// </code>
    /// </example>
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

        /// <summary>
        /// Inicializuje novou instanci generického handleru pro aktualizaci entit.
        /// </summary>
        /// <param name="repository">Repository implementující UpdateAsync a ExistsAsync metody</param>
        /// <param name="mapper">AutoMapper instance pro mapování DTO na entity</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <param name="existsAsyncFunc">Delegát pro ověření existence entity</param>
        /// <param name="updateAsyncFunc">Delegát pro aktualizaci entity</param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
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
        }

        /// <summary>
        /// Zpracuje command pro aktualizaci existující entity.
        /// </summary>
        /// <param name="dto">DTO obsahující data pro aktualizaci (již validovaná)</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <param name="handlerName">Název konkrétního handleru pro logování (např. "UpdateRaceHandler")</param>
        /// <returns>
        /// True pokud aktualizace proběhla úspěšně, jinak false.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Proces:
        /// <list type="number">
        /// <item><description>Loguje zahájení aktualizace</description></item>
        /// <item><description>Ověří existenci entity v databázi</description></item>
        /// <item><description>Mapuje DTO na domain entitu</description></item>
        /// <item><description>Aktualizuje entitu přes repository</description></item>
        /// <item><description>Loguje úspěšnou aktualizaci</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Pokud entita neexistuje, vyvolá <see cref="KeyNotFoundException"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Pokud entita s daným ID neexistuje</exception>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k chybě při mapování nebo aktualizaci</exception>
        protected async Task<TDto> HandleUpdate(
            TDto dto,
            int id,
            CancellationToken cancellationToken,
            string handlerName)
        {
            ILogUpdated logHelper = new LogHelpers<TEntity>(_logger, handlerName);;

            try
            {
                logHelper.LogUpdatingEntityInfo(id);

                cancellationToken.ThrowIfCancellationRequested();

                // Ověření existence entity
                bool exists = await _existsAsyncFunc(_repository, id, cancellationToken);

                if (!exists)
                {
                    logHelper.LogEntityNotFound(id);
                    logHelper.LogWarning("Return originalDto without update.");
                    return dto;
                }

                // Mapování DTO na domain entitu
                var entity = _mapper.Map<TEntity>(dto);

                // Aktualizace entity v repository
                var updatedEntity =  await _updateAsyncFunc(_repository, entity, cancellationToken);

                logHelper.LogEntityUpdatedSuccessfully(id);



                return _mapper.Map<TDto>(updatedEntity);
            }

            catch (OperationCanceledException)
            {
                logHelper.LogOperationCanceled(id);
                throw;
            }
            catch (Exception ex)
            {
                logHelper.LogUnexpectedError(ex, id);
                throw;
            }
        }
    }
}
