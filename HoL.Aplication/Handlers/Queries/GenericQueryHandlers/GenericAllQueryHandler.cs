using HoL.Domain.LogMessages;
using MediatR;

namespace HoL.Aplication.Handlers.Queries.GenericQueryes
{
    /// <summary>
    /// Generický MediatR query handler pro získání všech entit.
    /// </summary>
    /// <typeparam name="TEntity">Typ domain entity (např. Race, Character)</typeparam>
    /// <typeparam name="TDto">Typ DTO pro výstup (např. RaceDto, CharacterDto)</typeparam>
    /// <typeparam name="TRepository">Typ repository implementující ListAsync metodu</typeparam>
    public abstract class GenericAllQueryHandler<TEntity, TDto, TRepository>
        where TEntity : class
        where TDto : class
        where TRepository : class
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Func<TRepository, CancellationToken, Task<IEnumerable<TEntity>>> _getAllFunc;
        private readonly string _entityName;

        protected GenericAllQueryHandler(TRepository repository,
                                         IMapper mapper,
                                         ILogger logger,
                                         Func<TRepository, CancellationToken, Task<IEnumerable<TEntity>>> getAllFunc)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getAllFunc = getAllFunc ?? throw new ArgumentNullException(nameof(getAllFunc));
            _entityName = typeof(TEntity).Name;
        }

        protected async Task<IEnumerable<TDto>> HandleGetAll(CancellationToken cancellationToken, string handlerName)
        {
            _logger.LogInformation(LogMessageTemplates.GetAll.RetrievingAllEntities(handlerName, _entityName));

            try
            {
                var entities = await _getAllFunc(_repository, cancellationToken);
                
                if (!entities.Any())
                {
                    _logger.LogInformation(LogMessageTemplates.GetByIds.NoEntitiesFound(handlerName, _entityName));
                    return Enumerable.Empty<TDto>();
                }

                var entityList = entities.ToList();
                var dtoList = _mapper.Map<IEnumerable<TDto>>(entityList);
                var dtoListCount = dtoList.Count();

                _logger.LogInformation(
                    LogMessageTemplates.GetAll.EntitiesRetrievedSuccessfully(handlerName, dtoListCount, _entityName)
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
