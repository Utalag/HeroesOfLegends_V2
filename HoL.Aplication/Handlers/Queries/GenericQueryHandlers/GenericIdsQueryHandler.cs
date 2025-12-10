using HoL.Domain.LogMessages;

namespace HoL.Aplication.Handlers.Queries.GenericQueryes
{
    /// <summary>
    /// Generický MediatR query handler pro získání entit podle kolekce ID.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity (např. Race, Character)</typeparam>
    /// <typeparam name="TDto">Typ DTO pro výstup (např. RaceDto, CharacterDto)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující základní CRUD operace</typeparam>
    public abstract class GenericIdsQueryHandler<TEntity, TDto, TRepository>
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
        /// <param name="logger">Logger pro zaznamování operací</param>
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
            _entityName = typeof(TEntity).Name;
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
            if (ids is null)
            {
                
                return Enumerable.Empty<TDto>();
            }

            var normalizedIds = ids
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (!normalizedIds.Any())
            {
                
                return Enumerable.Empty<TDto>();
            }

           

            var foundEntities = new List<TEntity>(normalizedIds.Count);

            try
            {
                foreach (var id in normalizedIds)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var entity = await _getByIdFunc(_repository, id, cancellationToken);
                    if (entity is null)
                    {
                        continue;
                    }

                    foundEntities.Add(entity);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            if (foundEntities.Count == 0)
            {
                return Enumerable.Empty<TDto>();
            }

            var dtoList = _mapper.Map<List<TDto>>(foundEntities);

            

            return dtoList;
        }
    }
}
