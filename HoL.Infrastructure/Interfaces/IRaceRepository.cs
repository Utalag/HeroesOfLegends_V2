using HoL.Domain.Entities;
using HoL.Domain.Enums;

namespace HoL.Infrastructure.Interfaces
{
    public interface IRaceRepository //: IGenericRepository<Race, int>
    {
        Task<int> AddAsync(Race entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<Race?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Race>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
        Task<Race?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Race>> GetBySeqencAsync(int page, int size, string? sortBy = null, SortDirection direction = SortDirection.Original, CancellationToken cancellationToken = default);
        Task<IEnumerable<Race>> GetPageAsync(int page = 1, int size = 5, CancellationToken cancellationToken = default);
        Task<IEnumerable<Race>> ListAsync(CancellationToken cancellationToken = default);
        Task<int> UpdateAsync(Race entity, CancellationToken cancellationToken = default);
    }
}
