using System.Diagnostics;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Queries.GenericQueryes;
using HoL.Aplication.Handlers.Responses;
using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.Entities;
using HoL.Domain.LogMessages;
using Microsoft.AspNetCore.Http;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.AllRaces
{

    public class GetRaceAllQueryHandler : IRequestHandler<GetRaceAllQuery, Response<IEnumerable<RaceDto>>>
    {
        private readonly IRaceRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetRaceAllQueryHandler(IRaceRepository repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Response<IEnumerable<RaceDto>>> Handle(GetRaceAllQuery request, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            var traceId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("N");


            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var race = await _repository.ListAsync();

                if (!race.Any())
                {
                    sw.Stop();
                    return Response<IEnumerable<RaceDto>>.NoContent(
                        eventId: LogEventIds.QueryNotFoundHandled,
                        traceId: traceId,
                        elapsedMs: sw.ElapsedMilliseconds);
                }
                var raceDto = _mapper.Map <IEnumerable<RaceDto>>(race);

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
