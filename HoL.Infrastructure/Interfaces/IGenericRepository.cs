using System;
using System.Collections.Generic;
using System.Text;
using HoL.Domain.Entities;
using HoL.Domain.Enums;

namespace HoL.Infrastructure.Interfaces
{
    internal interface IGenericRepository<TEntity, TKey>
        where TEntity : class
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);
        Task<TEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetPageAsync(
            int Page = 1,
            int Size = 5,
            CancellationToken cancellationToken = default);
    }
}
