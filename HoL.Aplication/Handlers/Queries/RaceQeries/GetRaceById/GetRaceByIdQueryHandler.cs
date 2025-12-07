using System.Diagnostics;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Responses;
using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.LogMessages;
using Microsoft.AspNetCore.Http;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetRaceById
{

    public class GetRaceByIdQueryHandler : IRequestHandler<GetRaceByIdQuery,Response<RaceDto?>>
    {
        private readonly IRaceRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetRaceByIdQueryHandler(IRaceRepository repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        async Task<Response<RaceDto>> IRequestHandler<GetRaceByIdQuery, Response<RaceDto?>>.Handle(GetRaceByIdQuery request, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            var traceId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("N");

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var race = await _repository.GetByIdAsync(request.Id, cancellationToken);

                if (race == null)
                {
                    sw.Stop();
                    return Response<RaceDto>.NoContent(
                        eventId: LogEventIds.QueryNotFoundHandled,
                        traceId: traceId,
                        elapsedMs: sw.ElapsedMilliseconds);
                }
                var raceDto = _mapper.Map<RaceDto>(race);

                sw.Stop();
                return Response<RaceDto>.Ok(raceDto,
                    eventId: LogEventIds.QueryHandled,
                    traceId: traceId,
                    elapsedMs: sw.ElapsedMilliseconds);

            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                return Response<RaceDto>.Canceled(
                    eventId: LogEventIds.QueryCenceled,
                    traceId: traceId,
                    statusCode: 499,
                    elapsedMs: sw.ElapsedMilliseconds);
                throw;
            }

            catch (Exception)
            {
                return Response<RaceDto>.Fail(
                    error: "An error occurred while processing the request.",
                    eventId: LogEventIds.QueryFailed,
                    traceId: traceId,
                    elapsedMs: sw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}

