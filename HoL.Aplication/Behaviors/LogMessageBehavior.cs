using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Aplication.Handlers.Responses;
using HoL.Domain.Enums.Logging;
using Microsoft.AspNetCore.Http;


namespace HoL.Aplication.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger,
                               IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            var traceId = _httpContextAccessor.HttpContext?.Items["TraceId"] as string ?? Guid.NewGuid().ToString("N");

            try
            {
                _logger.LogInformation("Handling {Request} Trace {TraceId}", typeof(TRequest).Name, traceId);

                var response = await next();

                sw.Stop();

                switch (response)
                {
                    case Response r:
                        var levelR = r.Success ? LogLevel.Information : LogLevel.Warning;
                        _logger.Log(levelR, r.EventId ?? new EventId(0, "Response"),
                            "Handled {Request} in {Elapsed}ms Status {StatusCode} Success={Success} Trace {TraceId}",
                            typeof(TRequest).Name, sw.ElapsedMilliseconds, r.StatusCode, r.Success, traceId);
                        break;

                    case Response<object> rt:
                        var levelRT = rt.Success ? LogLevel.Information : LogLevel.Warning;
                        _logger.Log(levelRT, rt.EventId ?? new EventId(0, "Response<T>"),
                            "Handled {Request} in {Elapsed}ms Status {StatusCode} Success={Success} Trace {TraceId}",
                            typeof(TRequest).Name, sw.ElapsedMilliseconds, rt.StatusCode, rt.Success, traceId);
                        break;

                    default:
                        _logger.LogInformation("Handled {Request} in {Elapsed}ms Trace {TraceId}",
                            typeof(TRequest).Name, sw.ElapsedMilliseconds, traceId);
                        break;
                }

                return response;
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(new EventId((int)EventVariantType.UnhandledException, nameof(EventVariantType.UnhandledException)), ex,
                    "Error handling {Request} after {Elapsed}ms Trace {TraceId}",
                    typeof(TRequest).Name, sw.ElapsedMilliseconds, traceId);

                throw;
            }
        }
    }
}
