using Microsoft.Extensions.Logging;


namespace HoL.Infrastructure.Logging
{
    public class RepositoryLog<Tentity>
        where Tentity : class
    {
        public bool Success { get; init; }
        public string? TraceId { get; init; }
        public EventId? LogId { get; init; }
        public long ElapsedMs { get; init; }
        public string? ErrorMessage { get; init; }
        public Exception? Exception { get; init; }

        #region Factory Methods

        public static RepositoryLog<Tentity> OK(
            EventId? eventId = null,
            string? traceId = null,
            long elapsedMs = 0) =>
            new()
            {
                Success = true,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                LogId = eventId
            };
        public static RepositoryLog<Tentity> Failed(
            Exception ex,
            EventId? eventId = null,
            string? traceId = null,
            long elapsedMs = 0) =>
            new()
            {
                Success = false,
                ErrorMessage = ex.Message,
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                LogId = eventId,
                Exception = ex
            };

        public static RepositoryLog<Tentity> Canceled(
            EventId? eventId = null,
            string? traceId = null,
            long elapsedMs = 0) =>
            new()
            {
                Success = false,
                ErrorMessage = "Operation canceled",
                TraceId = traceId,
                ElapsedMs = elapsedMs,
                LogId = eventId,
            };

        #endregion

        // Logovací metoda - určuje, jak se log zapíše
        public void LogResult(ILogger logger)
        {
            if (Success)
            {
                logger.Log(
                    LogLevel.Information,
                    LogId ?? new EventId(0),
                    "{Entity}  successful. Trace {TraceId} Elapsed {Elapsed}ms",
                    typeof(Tentity).Name, TraceId, ElapsedMs);
            }
            else
            {
                logger.Log(
                    LogLevel.Error,
                    LogId ?? new EventId(0),
                    "{Entity} error. Trace {TraceId} Elapsed {Elapsed}ms | Failed message: { Error}, Exceprion: {exception}",
                    typeof(Tentity).Name, TraceId, ElapsedMs, ErrorMessage, Exception);

            }
        }


    }
}
