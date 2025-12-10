using HoL.Contracts;
using HoL.Domain.Entities;
using HoL.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace HoL.Infrastructure.Repositories
{
    public class RaceRepository : GenericRepository<Race, int>, IRaceRepository
    {
        public RaceRepository(SqlDbContext db, ILogger logger, IHttpContextAccessor httpContextAccessor) : base(db, logger, httpContextAccessor)
        {
        }
    }
}
