using System.Diagnostics;
using AutoMapper;
using HoL.Domain.Entities.CurencyEntities;
using HoL.Domain.Enums.Logging;
using HoL.Domain.LogMessages;
using HoL.Infrastructure.Data;
using HoL.Infrastructure.Data.MapModels;
using HoL.Infrastructure.Data.Models;
using HoL.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HoL.Infrastructure.Repositories
{
    /// <summary>
    /// Repository pro správu skupin měn v databázi.
    /// Mapuje CurrencyGroupDbModel na CurrencyGroup domain model.
    /// </summary>
    public class CurrencyGroupDbRepository
    {
        #region Fields and Constructor

        protected readonly SqlDbContext db;
        protected readonly DbSet<CurrencyGroupDbModel> dbSet;
        protected readonly ILogger logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CurrencyGroupDbRepository(
            SqlDbContext db,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.dbSet = db.Set<CurrencyGroupDbModel>();
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

        public async Task<int> AddAsync(CurrencyGroup entity, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Domain → Database mapping
                CurrencyGroupDbModel dbModel = _mapper.Map<CurrencyGroupDbModel>(entity);

                await dbSet.AddAsync(dbModel, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryAdd);
                var log = RepositoryLog<CurrencyGroup>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModel.Id;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<CurrencyGroup>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<int> UpdateAsync(CurrencyGroup entity, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // 1. Načíst existující entitu z databáze s related entities
                var existingDbModel = await dbSet
                    .Include(cg => cg.Currencies)
                    .FirstOrDefaultAsync(cg => cg.Id == entity.Id, cancellationToken);

                if (existingDbModel == null)
                    throw new KeyNotFoundException($"CurrencyGroup with Id {entity.Id} not found");

                // 2. Mapovat změny z domain entity na existující DB model
                _mapper.Map(entity, existingDbModel);

                // 3. EF Core automaticky detekuje změny v tracked entity
                await db.SaveChangesAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryUpdate);
                var log = RepositoryLog<CurrencyGroup>.OK(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.OK(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.OK(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<CurrencyGroup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                CurrencyGroupDbModel? dbModel = await dbSet
                    .Include(cg => cg.Currencies)
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<CurrencyGroup>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModel != null ? dbModel.MapToCurrencyGroup() : null;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<CurrencyGroup>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<IEnumerable<CurrencyGroup>> ListAsync(CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                List<CurrencyGroupDbModel> dbModels = await dbSet
                    .Include(cg => cg.Currencies)
                    .ToListAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<CurrencyGroup>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModels.Select(x => x.MapToCurrencyGroup()).ToList();
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<CurrencyGroup>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<CurrencyGroup?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                CurrencyGroupDbModel? dbModel = await dbSet
                    .Include(cg => cg.Currencies)
                    .FirstOrDefaultAsync(x => x.GroupName == name, cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<CurrencyGroup>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModel != null ? dbModel.MapToCurrencyGroup() : null;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<CurrencyGroup>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<IEnumerable<CurrencyGroup>> GetPageAsync(int page = 1, int size = 5, CancellationToken cancellationToken = default)
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
                    .Include(cg => cg.Currencies)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<CurrencyGroup>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModels.Select(x => x.MapToCurrencyGroup()).ToList();
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<CurrencyGroup>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public async Task<IEnumerable<CurrencyGroup>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var idList = ids.ToList();
                var dbModels = await dbSet
                    .Include(cg => cg.Currencies)
                    .Where(x => idList.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<CurrencyGroup>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return dbModels.Select(x => x.MapToCurrencyGroup()).ToList();
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<CurrencyGroup>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<CurrencyGroup>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        #endregion
    }
}
