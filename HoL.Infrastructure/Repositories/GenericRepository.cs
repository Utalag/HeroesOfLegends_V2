using HoL.Contracts;
using HoL.Domain.Enums.Logging;
using HoL.Domain.LogMessages;
using HoL.Infrastructure.Data;
using HoL.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

namespace HoL.Infrastructure.Repositories
{
    public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly SqlDbContext db;
        protected readonly DbSet<TEntity> dbSet;
        protected readonly ILogger logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected GenericRepository(SqlDbContext db, ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.dbSet = db.Set<TEntity>();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected string GetTraceId()
        {
            var httpContextAccesor = _httpContextAccessor.HttpContext;
            return httpContextAccesor?.TraceIdentifier ?? Guid.NewGuid().ToString("N");
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                await dbSet.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryAdd);
                var log = RepositoryLog<TEntity>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return entity;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logid = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<TEntity>.Canceled(logid, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<TEntity>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                dbSet.Update(entity);
                await db.SaveChangesAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryUpdate);
                var log = RepositoryLog<TEntity>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return entity;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<TEntity>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                db.Entry(entity).State = EntityState.Unchanged;
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
                var log = RepositoryLog<TEntity>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                db.Entry(entity).State = EntityState.Unchanged;
                throw;
            }
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                TEntity? entity = await dbSet.FindAsync(new object[] { id! }, cancellationToken);

                if (entity is null)
                {
                    sw.Stop();
                    return;
                }

                dbSet.Remove(entity);
                await db.SaveChangesAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryDelete);
                var log = RepositoryLog<TEntity>.OK(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<TEntity>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<TEntity>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                TEntity? entity = await dbSet.FindAsync(new object[] { id! }, cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<TEntity>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                if (entity is not null)
                {
                    db.Entry(entity).State = EntityState.Detached;
                    return true;
                }

                return false;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<TEntity>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<TEntity>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var entity = await dbSet.FindAsync(new object[] { id! }, cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<TEntity>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return entity;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<TEntity>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<TEntity>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var entities = await dbSet.ToListAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<TEntity>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return entities;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<TEntity>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<TEntity>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public virtual async Task<TEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var property = Expression.Property(parameter, "RaceName");
                var constant = Expression.Constant(name);
                var equality = Expression.Equal(property, constant);
                var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

                var entity = await dbSet.FirstOrDefaultAsync(lambda, cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<TEntity>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return entity;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<TEntity>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<TEntity>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetPageAsync(
            int Page = 1,
            int Size = 5,
            CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                IQueryable<TEntity> query = dbSet;

                var entities = await query
                    .Skip((Page - 1) * Size)
                    .Take(Size)
                    .ToListAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<TEntity>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return entities;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<TEntity>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<TEntity>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var idList = ids.ToList();
                var entities = new List<TEntity>();

                foreach (var id in idList)
                {
                    var entity = await dbSet.FindAsync(new object[] { id! }, cancellationToken);
                    if (entity is not null)
                    {
                        entities.Add(entity);
                        db.Entry(entity).State = EntityState.Detached;
                    }
                }

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<TEntity>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return entities;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<TEntity>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<TEntity>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetBySeqencAsync(
            int page,
            int size,
            string? sortBy = null,
            string? sortDir = null,
            CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                IQueryable<TEntity> query = dbSet;

                // Řazení
                if (!string.IsNullOrEmpty(sortBy))
                {
                    var param = Expression.Parameter(typeof(TEntity), "x");
                    var property = Expression.Property(param, sortBy);
                    var lambda = Expression.Lambda(property, param);

                    if (sortDir?.ToLower() == "desc")
                    {
                        query = query.Provider.CreateQuery<TEntity>(
                            Expression.Call(
                                typeof(Queryable),
                                "OrderByDescending",
                                new[] { typeof(TEntity), property.Type },
                                query.Expression,
                                Expression.Quote(lambda)));
                    }
                    else
                    {
                        query = query.Provider.CreateQuery<TEntity>(
                            Expression.Call(
                                typeof(Queryable),
                                "OrderBy",
                                new[] { typeof(TEntity), property.Type },
                                query.Expression,
                                Expression.Quote(lambda)));
                    }
                }

                // Stránkování
                var entities = await query
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var log = RepositoryLog<TEntity>.OK(logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);

                return entities;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<TEntity>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<TEntity>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }
    }
}
