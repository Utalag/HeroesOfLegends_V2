using HoL.Domain.Enums;
using HoL.Domain.LogMessages;
using HoL.Infrastructure.Data;
using HoL.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace HoL.Infrastructure.Repositories
{
    public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly SqlDbContext db;
        protected readonly DbSet<TEntity> dbSet;
        protected readonly ILogger logger;
        private readonly string _sourceName;
        private readonly string _entityName;

        protected GenericRepository(SqlDbContext db, ILogger logger)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.dbSet = db.Set<TEntity>();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sourceName = GetType().Name;
            _entityName = typeof(TEntity).Name;
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            logger.LogInformation(LogMessageTemplates.Adding.AddingEntity(_sourceName, _entityName));
            
            try
            {
                await dbSet.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);
                
                logger.LogInformation(LogMessageTemplates.Adding.EntityAddedSuccessfully(_sourceName, _entityName));
            }
            catch (Exception ex)
            {
                logger.LogError(LogMessageTemplates.Exceptions.UnexpectedError(_sourceName, _entityName, ex));
                throw;
            }
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            logger.LogInformation(LogMessageTemplates.Updating.UpdatingEntity(_sourceName, _entityName));
            
            try
            {
                dbSet.Update(entity);
                await db.SaveChangesAsync(cancellationToken);
                
                logger.LogInformation(LogMessageTemplates.Updating.EntityUpdatedSuccessfully(_sourceName, _entityName));
            }
            catch (Exception ex)
            {
                logger.LogError(LogMessageTemplates.Exceptions.UnexpectedError(_sourceName, _entityName, ex));
                db.Entry(entity).State = EntityState.Unchanged;
                throw;
            }
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation(LogMessageTemplates.Deleting.DeletingEntityInfo(_sourceName, _entityName, id!));
            
            TEntity? entity = await dbSet.FindAsync(new object[] { id! }, cancellationToken);

            if (entity is null)
            {
                logger.LogWarning(LogMessageTemplates.Deleting.EntityNotFoundForDeletion(_sourceName, _entityName, id!));
                return;
            }

            try
            {
                dbSet.Remove(entity);
                await db.SaveChangesAsync(cancellationToken);
                
                logger.LogInformation(LogMessageTemplates.Deleting.EntityDeletedSuccessfully(_sourceName, _entityName, id!));
            }
            catch (Exception ex)
            {
                logger.LogError(LogMessageTemplates.Exceptions.UnexpectedErrorWithId(_sourceName, _entityName, id, ex));
                db.Entry(entity).State = EntityState.Unchanged;
                throw;
            }
        }

        public virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
        {
            TEntity? entity = await dbSet.FindAsync(new object[] { id! }, cancellationToken);
            
            if (entity is not null)
            {
                db.Entry(entity).State = EntityState.Detached;
                logger.LogInformation(LogMessageTemplates.Existence.EntityExists(_sourceName, _entityName, id!));
                return true;
            }
            
            logger.LogInformation(LogMessageTemplates.Existence.EntityDoesNotExist(_sourceName, _entityName, id!));
            return false;
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation(LogMessageTemplates.GetById.LookingForEntityById(_sourceName, _entityName, id!));
            
            try
            {
                var entity = await dbSet.FindAsync(new object[] { id! }, cancellationToken);
                
                if (entity == null)
                {
                    logger.LogWarning(LogMessageTemplates.GetById.EntityNotFound(_sourceName, _entityName, id!));
                }
                else
                {
                    logger.LogInformation(LogMessageTemplates.GetById.EntityRetrievedSuccessfully(_sourceName, _entityName, id!));
                }
                
                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(LogMessageTemplates.Exceptions.UnexpectedErrorWithId(_sourceName,_entityName,id,ex));
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default)
        {
            logger.LogInformation(LogMessageTemplates.GetAll.RetrievingAllEntities(_sourceName, _entityName));
            
            try
            {
                var entities = await dbSet.ToListAsync(cancellationToken);
                
                logger.LogInformation(LogMessageTemplates.GetAll.EntitiesRetrievedSuccessfully(_sourceName, entities.Count, _entityName));
                
                return entities;
            }
            catch (Exception ex)
            {
                   logger.LogError(LogMessageTemplates.Exceptions.UnexpectedError(_sourceName,_entityName,ex));
                throw;
            }
        }

        public virtual async Task<TEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            logger.LogInformation(LogMessageTemplates.GetByName.LookingForEntityByName(_sourceName, _entityName, name));
            
            try
            {
                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var property = Expression.Property(parameter, "RaceName");
                var constant = Expression.Constant(name);
                var equality = Expression.Equal(property, constant);
                var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

                var entity = await dbSet.FirstOrDefaultAsync(lambda, cancellationToken);
                
                if (entity == null)
                {
                    logger.LogWarning(LogMessageTemplates.GetByName.EntityNotFoundByName(_sourceName, _entityName, name));
                }
                else
                {
                    logger.LogInformation(LogMessageTemplates.GetByName.EntityRetrievedSuccessfullyByName(_sourceName, _entityName, name));
                }
                
                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LogMessageTemplates.GetByName.ErrorRetrievingEntityByName(_sourceName, _entityName, name));
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetPageAsync(
            int Page = 1, 
            int Size = 5, 
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation(LogMessageTemplates.Pagination.RetrievingPage(_sourceName, _entityName, Page, Size));
            
            try
            {
                IQueryable<TEntity> query = dbSet;

                var entities = await query
                    .Skip((Page - 1) * Size)
                    .Take(Size)
                    .ToListAsync(cancellationToken);
                
                logger.LogInformation(LogMessageTemplates.Pagination.PageRetrievedSuccessfully(_sourceName, entities.Count, _entityName, Page));
                
                return entities;
            }
            catch (Exception ex)
            {
                logger.LogError(LogMessageTemplates.Exceptions.UnexpectedError(_sourceName, _entityName, ex));
                throw;
            }
        }
    }
}
