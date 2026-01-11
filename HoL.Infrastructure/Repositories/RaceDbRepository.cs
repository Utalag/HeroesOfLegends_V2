using System.Diagnostics;
using AutoMapper;
using HoL.Domain.Entities;
using HoL.Domain.Enums;
using HoL.Domain.Enums.Logging;
using HoL.Domain.LogMessages;
using HoL.Infrastructure.Data;
using HoL.Infrastructure.Data.MapModels;
using HoL.Infrastructure.Data.Models;
using HoL.Infrastructure.Interfaces;
using HoL.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HoL.Infrastructure.Repositories
{
    public class RaceDbRepository : IRaceRepository
    {

        #region Fields and Constructor

        protected readonly SqlDbContext db;
        protected readonly DbSet<RaceDbModel> dbSet;
        protected readonly ILogger logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public RaceDbRepository(
            SqlDbContext db,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.dbSet = db.Set<RaceDbModel>();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected string GetTraceId()
        {
            var httpContextAccessor = _httpContextAccessor.HttpContext;
            return httpContextAccessor?.TraceIdentifier ?? Guid.NewGuid().ToString("N");
        }

        #endregion

        #region Write Operations

        public async Task<int> AddAsync(Race entity, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Domain → Database mapping
                RaceDbModel dbModel = _mapper.Map<RaceDbModel>(entity);

                await dbSet.AddAsync(dbModel, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryAdd);
                var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModel.Id;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logid = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Race>.Canceled(logid, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Error,
                    EventVariantType.UnhandledException);
                var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<int> UpdateAsync(Race entity, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // 1. Načíst existující entitu z databáze s related entities
                var existingDbModel = await dbSet
                    .Include(r => r.Treasure)
                        .ThenInclude(t => t.CurrencyGroup)
                            .ThenInclude(cg => cg.Currencies)
                    .FirstOrDefaultAsync(r => r.Id == entity.Id, cancellationToken);

                if (existingDbModel == null)
                    throw new KeyNotFoundException($"Race with Id {entity.Id} not found");

                // 2. Mapovat změny z domain entity na existující DB model
                // POZNÁMKA: AutoMapper aktualizuje existující objekt místo vytvoření nového
                _mapper.Map(entity, existingDbModel);

                // 3. EF Core automaticky detekuje změny v tracked entity
                // Není potřeba volat dbSet.Update()!
                await db.SaveChangesAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryUpdate);
                var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return existingDbModel.Id;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Race>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Error,
                    EventVariantType.UnhandledException);
                var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var dbModel = await dbSet.FindAsync(new object[] { id }, cancellationToken);

                if (dbModel is null)
                {
                    sw.Stop();
                    return;
                }

                dbSet.Remove(dbModel);
                await db.SaveChangesAsync(cancellationToken);
                sw.Stop();

                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryDelete);
                var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Race>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Error,
                    EventVariantType.UnhandledException);
                var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        #endregion

        #region Read Operations

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var exists = await dbSet.AnyAsync(x => x.Id == id, cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return exists;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Race>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Error,
                    EventVariantType.UnhandledException);
                var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<Race?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Eager Loading - načítá Treasure s CurrencyGroup a Currencies
                RaceDbModel? dbModel = await dbSet
                    .Include(r => r.Treasure)
                        .ThenInclude(t => t.CurrencyGroup)
                            .ThenInclude(cg => cg.Currencies)
                    .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModel != null ? _mapper.Map<Race>(dbModel) : null;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Race>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Error,
                    EventVariantType.UnhandledException);
                var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<IEnumerable<Race>> ListAsync(CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                List<RaceDbModel> dbModels = await dbSet
                    .Include(r => r.Treasure)
                        .ThenInclude(t => t.CurrencyGroup)
                            .ThenInclude(cg => cg.Currencies)
                    .ToListAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModels.Select(x => _mapper.Map<Race>(x)).ToList();
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Race>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Error,
                    EventVariantType.UnhandledException);
                var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<Race?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                RaceDbModel? dbModel = await dbSet
                    .Include(r => r.Treasure)
                        .ThenInclude(t => t.CurrencyGroup)
                            .ThenInclude(cg => cg.Currencies)
                    .FirstOrDefaultAsync(x => x.RaceName == name, cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModel != null ? _mapper.Map<Race>(dbModel) : null;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Race>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Error,
                    EventVariantType.UnhandledException);
                var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<IEnumerable<Race>> GetPageAsync(int page = 1, int size = 5, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (page < 1)
                    page = 1;
                if (size < 1)
                    size = 5;
                if (size > 100)
                    size = 100;

                var dbModels = await dbSet
                    .Include(r => r.Treasure)
                        .ThenInclude(t => t.CurrencyGroup)
                            .ThenInclude(cg => cg.Currencies)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModels.Select(x => _mapper.Map<Race>(x)).ToList();
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Race>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Error,
                    EventVariantType.UnhandledException);
                var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<IEnumerable<Race>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var idList = ids.ToList();
                var dbModels = await dbSet
                    .Include(r => r.Treasure)
                        .ThenInclude(t => t.CurrencyGroup)
                            .ThenInclude(cg => cg.Currencies)
                    .Where(x => idList.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModels.Select(x => _mapper.Map<Race>(x)).ToList();
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Race>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Error,
                    EventVariantType.UnhandledException);
                var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<IEnumerable<Race>> GetBySeqencAsync(int page, int size, string? sortBy = null, SortDirection direction = SortDirection.Original, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Validace sortBy - pouze povolené vlastnosti
                var allowedProperties = new[]
                {
                    nameof(RaceDbModel.Id),
                    nameof(RaceDbModel.RaceName),
                    nameof(RaceDbModel.RaceCategory),
                    nameof(RaceDbModel.BaseInitiative),
                    nameof(RaceDbModel.BaseXP),
                    nameof(RaceDbModel.FightingSpiritNumber),
                    nameof(RaceDbModel.ZSM),
                    nameof(RaceDbModel.DomesticationValue)
                };

                // Pokud není zadán atribut, řadíme podle Id
                var attributeName = string.IsNullOrEmpty(sortBy) ? nameof(RaceDbModel.Id) : sortBy;

                // Ověřit, zda je vlastnost povolena
                if (!allowedProperties.Contains(attributeName, StringComparer.OrdinalIgnoreCase))
                {
                    // Pokud je sortBy neplatný, vratíme bez řazení
                    attributeName = nameof(RaceDbModel.Id);
                    direction = SortDirection.Original;
                }

                if (page < 1)
                    page = 1;
                if (size < 1)
                    size = 5;
                if (size > 100)
                    size = 100;

                // Aplikace řazení podle směru
                IQueryable<RaceDbModel> query = direction switch
                {
                    SortDirection.Original => dbSet
                        .Include(r => r.Treasure)
                            .ThenInclude(t => t.CurrencyGroup)
                                .ThenInclude(cg => cg.Currencies),
                    SortDirection.Ascending => dbSet
                        .Include(r => r.Treasure)
                            .ThenInclude(t => t.CurrencyGroup)
                                .ThenInclude(cg => cg.Currencies)
                        .OrderBy(x => EF.Property<object>(x, attributeName)),
                    SortDirection.Descending => dbSet
                        .Include(r => r.Treasure)
                            .ThenInclude(t => t.CurrencyGroup)
                                .ThenInclude(cg => cg.Currencies)
                        .OrderByDescending(x => EF.Property<object>(x, attributeName)),
                    _ => dbSet
                        .Include(r => r.Treasure)
                            .ThenInclude(t => t.CurrencyGroup)
                                .ThenInclude(cg => cg.Currencies)
                };

                // Stránkování
                var result = await query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<Race>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return result.Select(x => _mapper.Map<Race>(x)).ToList();
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Race>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Error,
                    EventVariantType.UnhandledException);
                var log = RepositoryLog<Race>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        #endregion
    }
}
