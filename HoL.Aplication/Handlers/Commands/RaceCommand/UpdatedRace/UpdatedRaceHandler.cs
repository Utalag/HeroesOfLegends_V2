using HoL.Aplication.Handlers.Responses;
using HoL.Contracts;
using HoL.Domain.Entities;
using HoL.Domain.Enums.Logging;
using HoL.Domain.LogMessages;
using Microsoft.AspNetCore.Http;

namespace HoL.Aplication.Handlers.Commands.RaceCommand.UpdatedRace
{
    /// <summary>
    /// Handler pro aktualizaci existující rasy.
    /// Validace probíhá automaticky přes FluentValidation pipeline (ValidationBehavior).
    /// </summary>
    public class UpdatedRaceHandler : IRequestHandler<UpdatedRaceCommand, Response<int>>
    {
        private readonly IRaceRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatedRaceHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdatedRaceHandler(
            IRaceRepository repository,
            IMapper mapper,
            ILogger<UpdatedRaceHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Response<int>> Handle(UpdatedRaceCommand request, CancellationToken cancellationToken)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var traceId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("N");

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var exists = await _repository.ExistsAsync(request.RaceDto.RaceId, cancellationToken);

                if (!exists)
                {
                    sw.Stop();
                    return Response<int>.NoContent(
                        eventId: LogIdFactory.Create(ProjectLayerType.Application, OperationType.Command, LogLevelCodeType.Information, EventVariantType.QueryNotFound),
                        traceId: traceId,
                        elapsedMs: sw.ElapsedMilliseconds);
                }

                var domain = _mapper.Map<Race>(request.RaceDto);
                await _repository.UpdateAsync(domain, cancellationToken);

                sw.Stop();
                return Response<int>.Ok(
                    data: domain.Id,
                    eventId: LogEventIds.CommandUpdated,
                    traceId: traceId,
                    elapsedMs: sw.ElapsedMilliseconds);
            }
            catch (OperationCanceledException)
            {

                sw.Stop();
                return Response<int>.Canceled(
                    eventId: LogEventIds.CommandCanceled,
                    traceId: traceId,
                    elapsedMs: sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {

                sw.Stop();
                return Response<int>.Fail(
                    error: ex.Message,
                    eventId: LogEventIds.CommandFailed,
                    traceId: traceId,
                    elapsedMs: sw.ElapsedMilliseconds);
            }
        }
    }
}

