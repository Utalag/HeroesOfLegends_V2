using System.Diagnostics;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Responses;
using HoL.Domain.LogMessages;
using HoL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetRaceByIds
{

    public class GetRaceByIdsQueryHandler : IRequestHandler<GetRacesByIdsQuery, Response<IEnumerable<RaceDto>>>
    {
        private readonly IRaceRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetRaceByIdsQueryHandler(IRaceRepository repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<Response<IEnumerable<RaceDto>>> Handle(GetRacesByIdsQuery request, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            var traceId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("N");

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var races = await _repository.GetByIdsAsync(request.Ids, cancellationToken);

                if (!races.Any())
                {
                    sw.Stop();
                    return Response<IEnumerable<RaceDto>>.NoContent(
                        eventId: LogEventIds.QueryNotFoundHandled,
                        traceId: traceId,
                        elapsedMs: sw.ElapsedMilliseconds);
                }
                var raceDto = _mapper.Map<IEnumerable<RaceDto>>(races);
                sw.Stop();
                return Response<IEnumerable<RaceDto>>.Ok(raceDto,
                    eventId: LogEventIds.QueryHandled,
                    traceId: traceId,
                    elapsedMs: sw.ElapsedMilliseconds);

            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                return Response<IEnumerable<RaceDto>>.Canceled(
                    eventId: LogEventIds.QueryCenceled,
                    traceId: traceId,
                    statusCode: 499,
                    elapsedMs: sw.ElapsedMilliseconds);
                throw;
            }

            catch (Exception)
            {
                return Response<IEnumerable<RaceDto>>.Fail(
                    error: "An error occurred while processing the request.",
                    eventId: LogEventIds.QueryFailed,
                    traceId: traceId,
                    elapsedMs: sw.ElapsedMilliseconds);
                throw;
            }
        }
    }


}
