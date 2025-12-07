using HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace;
using HoL.Aplication.Handlers.Responses;
using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.Enums.Logging;
using HoL.Domain.LogMessages;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.RemoveRace
{
    public class RemoveRaceHandler : IRequestHandler<RemoveRaceCommand, Response>
    {
        private readonly IRaceRepository _repository;
        private readonly ILogger<RemoveRaceHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveRaceHandler(
            IRaceRepository repository,
            ILogger<RemoveRaceHandler> logger,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Response> Handle(RemoveRaceCommand request, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            var traceId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("N");

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var exists = await _repository.ExistsAsync(request.RaceDto.RaceId, cancellationToken);

                if (!exists)
                {
                    sw.Stop();
                    return Response.NoContent(
                        eventId: LogIdFactory.Create(ProjectLayerType.Application, OperationType.Command, LogLevelCodeType.Information, EventVariantType.QueryNotFound),
                        traceId: traceId,
                        elapsedMs: sw.ElapsedMilliseconds);
                }
                // removed
                await _repository.DeleteAsync(request.RaceDto.RaceId, cancellationToken);
                sw.Stop();
                return Response.Ok(
                    eventId: LogEventIds.CommandDeleted,
                    traceId: traceId,
                    elapsedMs: sw.ElapsedMilliseconds);

            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                return Response.Canceled(
                    eventId: LogEventIds.CommandCanceled,
                    traceId: traceId,
                    elapsedMs: sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                return Response.Fail(
                    error: ex.Message,
                    eventId: LogEventIds.CommandFailed,
                    traceId: traceId,
                    elapsedMs: sw.ElapsedMilliseconds);
            }
        }
    }
}
