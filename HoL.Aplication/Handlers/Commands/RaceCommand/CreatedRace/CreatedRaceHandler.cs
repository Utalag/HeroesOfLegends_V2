using HoL.Aplication.Handlers.Responses;
using HoL.Contracts;
using HoL.Domain.Entities;
using HoL.Domain.LogMessages;
using Microsoft.AspNetCore.Http;

namespace HeroesOfLegends.Application.Handlers.Commands.RaceCommand.CreatedRace
{
    public class CreatedRaceHandler : IRequestHandler<CreatedRaceCommand, Response<int>>
    {
        private readonly IRaceRepository _repository;
        private readonly ILogger<CreatedRaceHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatedRaceHandler(
            IRaceRepository repository,
            ILogger<CreatedRaceHandler> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Response<int>> Handle(CreatedRaceCommand request, CancellationToken cancellationToken)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var traceId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("N");

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var domain = _mapper.Map<Race>(request.RaceDto);
                await _repository.AddAsync(domain, cancellationToken);

                sw.Stop();
                return Response<int>.Ok(
                    data: domain.Id,
                    eventId: LogEventIds.CommandCreated,
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