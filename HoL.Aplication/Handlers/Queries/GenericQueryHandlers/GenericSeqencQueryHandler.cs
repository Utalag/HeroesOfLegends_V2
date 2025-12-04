using HoL.Domain.LogMessages;
using HoL.Domain.Enums;

namespace HoL.Aplication.Handlers.Queries.GenericQueryes
{
    /// <summary>
    /// Generický MediatR query handler pro sekvenční (stránkované) získání entit s možností řazení.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity (např. Race, Character)</typeparam>
    /// <typeparam name="TDto">Typ DTO pro výstup (např. RaceDto, CharacterDto)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující GetBySeqencAsync metodu</typeparam>
    public abstract class GenericSeqencQueryHandler<TEntity, TDto, TRepository>
        where TEntity : class
        where TDto : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Func<TRepository, int, int, string?, SortDirection, CancellationToken, Task<IEnumerable<TEntity>>> _getSequenceDataFunc;
        private readonly string _entityName;

        protected GenericSeqencQueryHandler(TRepository repository,
                                     IMapper mapper,
                                     ILogger logger,
                                     Func<TRepository, int, int, string?, SortDirection, CancellationToken, Task<IEnumerable<TEntity>>> getSequenceDataFunc)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getSequenceDataFunc = getSequenceDataFunc ?? throw new ArgumentNullException(nameof(getSequenceDataFunc));
            _entityName = typeof(TEntity).Name;
        }

        protected async Task<IEnumerable<TDto>> HandleSeqencData(
                                                                 int page,
                                                                 int size,
                                                                 string? sortBy,
                                                                 SortDirection sortDir,
                                                                 CancellationToken cancellationToken,
                                                                 string handlerName)
        {
            try
            {
                _logger.LogInformation(
                    LogMessageTemplates.Pagination.RetrievingPageWithSort(handlerName, _entityName, page, size, sortBy ?? "default", sortDir.ToString())
                );

                var entities = await _getSequenceDataFunc(_repository, page, size, sortBy, sortDir, cancellationToken);

                if (entities == null || !entities.Any())
                {
                    _logger.LogInformation(LogMessageTemplates.GetByIds.NoEntitiesFound(handlerName, _entityName));
                    return Enumerable.Empty<TDto>();
                }

                var entityList = entities.ToList();
                var dtoList = _mapper.Map<IEnumerable<TDto>>(entityList);
                var dtoCount = dtoList.Count();

                _logger.LogInformation(
                    LogMessageTemplates.Pagination.SequenceQueryCompleted(handlerName, dtoCount, _entityName)
                );
                
                return dtoList;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning(LogMessageTemplates.Exceptions.OperationCanceled(handlerName));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessageTemplates.Exceptions.UnexpectedError(handlerName, _entityName, ex));
                throw;
            }
        }
    }
}
