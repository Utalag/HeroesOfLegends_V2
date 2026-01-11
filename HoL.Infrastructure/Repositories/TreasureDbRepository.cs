using System.Diagnostics;
using System.Text.Json;
using AutoMapper;
using HoL.Domain.ValueObjects;
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
    /// Repository pro správu pokladů v databázi.
    /// Pozn: Treasure je value object vlastněn Race, takže repository je specifický pro čtení.
    /// Mapuje TreasureDbModel na Treasure domain model.
    /// </summary>
    public class TreasureDbRepository
    {
        #region Fields and Constructor

        protected readonly SqlDbContext db;
        protected readonly DbSet<RaceDbModel> raceDbSet;
        protected readonly ILogger logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public TreasureDbRepository(
            SqlDbContext db,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.raceDbSet = db.Set<RaceDbModel>();
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

        #region Read Operations

        /// <summary>
        /// Získá poklad pro danou rasu podle ID.
        /// </summary>
        public async Task<Treasure?> GetByRaceIdAsync(int raceId, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Načíst Race s Treasure a CurrencyGroup pomocí Eager Loading
                var raceDbModel = await raceDbSet
                    .Include(r => r.Treasure)
                        .ThenInclude(t => t.CurrencyGroup)
                            .ThenInclude(cg => cg.Currencies)
                    .FirstOrDefaultAsync(x => x.Id == raceId, cancellationToken);

                if (raceDbModel?.Treasure == null)
                {
                    sw.Stop();
                    var logId = LogIdFactory.Create(
                        ProjectLayerType.Infrastructure,
                        OperationType.Repository,
                        LogLevelCodeType.Information,
                        EventVariantType.RepositoryRead);
                    var log = RepositoryLog<Treasure>.OK(logId, traceId, sw.ElapsedMilliseconds);
                    log.LogResult(logger);
                    return null;
                }

                if (raceDbModel.Treasure.CurrencyGroup == null)
                {
                    sw.Stop();
                    throw new InvalidOperationException(
                        $"Měnová skupina pro poklad rasy {raceId} nebyla načtena.");
                }

                // Mapování s již načtenou CurrencyGroup
                var treasure = raceDbModel.Treasure.MapToTreasure();

                sw.Stop();
                var logIdSuccess = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryRead);
                var logSuccess = RepositoryLog<Treasure>.OK(logIdSuccess, traceId, sw.ElapsedMilliseconds);
                logSuccess.LogResult(logger);

                return treasure;
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Warning,
                    EventVariantType.TimeoutOccurred);
                var log = RepositoryLog<Treasure>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<Treasure>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        /// <summary>
        /// Aktualizuje poklad pro danou rasu.
        /// Pozn: Treasure je value object, proto se aktualizuje pouze JSON obsah.
        /// </summary>
        public async Task UpdateTreasureAsync(
            int raceId,
            Treasure treasure,
            CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var traceId = GetTraceId();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Načíst Race s Treasure
                var raceDbModel = await raceDbSet
                    .FirstOrDefaultAsync(x => x.Id == raceId, cancellationToken);

                if (raceDbModel?.Treasure == null)
                {
                    sw.Stop();
                    throw new InvalidOperationException(
                        $"Poklad pro rasu s ID {raceId} nebyl nalezen.");
                }

                // Aktualizace JSON dat
                raceDbModel.Treasure.CoinQuantitiesJson = JsonSerializer.Serialize(
                    treasure.CoinQuantities,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                await db.SaveChangesAsync(cancellationToken);

                sw.Stop();
                var logId = LogIdFactory.Create(
                    ProjectLayerType.Infrastructure,
                    OperationType.Repository,
                    LogLevelCodeType.Information,
                    EventVariantType.RepositoryUpdate);
                var log = RepositoryLog<Treasure>.OK(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<Treasure>.Canceled(logId, traceId, sw.ElapsedMilliseconds);
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
                var log = RepositoryLog<Treasure>.Failed(ex, logId, traceId, sw.ElapsedMilliseconds);
                log.LogResult(logger);
                throw;
            }
        }

        #endregion
    }
}
