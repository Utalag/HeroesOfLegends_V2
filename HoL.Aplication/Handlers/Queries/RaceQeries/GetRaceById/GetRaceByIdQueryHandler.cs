using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Interfaces.IRerpositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetRaceById
{
    /// <summary>
    /// MediatR query handler pro získání rasy podle ID.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Handler provádí:
    /// <list type="number">
    /// <item><description>Validace vstupu (automaticky přes ValidationBehavior)</description></item>
    /// <item><description>Načtení Race entity z databáze</description></item>
    /// <item><description>Mapování na RaceDto</description></item>
    /// <item><description>Vrácení výsledku nebo null pokud nenalezeno</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Query je read-only operace - nemění stav systému.
    /// </para>
    /// </remarks>
    /// <seealso cref="GetRaceByIdQuery"/>
    /// <seealso cref="GetRaceByIdQueryValidator"/>
    /// <seealso cref="IRaceRepository"/>
    public class GetRaceByIdQueryHandler : IRequestHandler<GetRaceByIdQuery, RaceDto?>
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRaceByIdQueryHandler> _logger;

        /// <summary>
        /// Inicializuje novou instanci <see cref="GetRaceByIdQueryHandler"/>
        /// </summary>
        /// <param name="raceRepository">Repository pro přístup k Race entitám</param>
        /// <param name="mapper">AutoMapper instance pro mapování entity na DTO</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
        public GetRaceByIdQueryHandler(
            IRaceRepository raceRepository,
            IMapper mapper,
            ILogger<GetRaceByIdQueryHandler> logger)
        {
            _raceRepository = raceRepository ?? throw new ArgumentNullException(nameof(raceRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Zpracuje query pro získání rasy podle ID.
        /// </summary>
        /// <param name="request">Query s ID rasy</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>
        /// <see cref="RaceDto"/> pokud rasa existuje, jinak <c>null</c>.
        /// </returns>
        /// <exception cref="ValidationException">
        /// Pokud validace selže (vyvoláno ValidationBehavior před tímto handlerem)
        /// </exception>
        /// <exception cref="DbException">Pokud dojde k chybě při čtení z databáze</exception>
        public async Task<RaceDto?> Handle(GetRaceByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Querying Race with Id: {RaceId}", request.Id);

            try
            {
                // Načtení entity z databáze
                var raceEntity = await _raceRepository.GetByIdAsync(request.Id, cancellationToken);

                if (raceEntity == null)
                {
                    _logger.LogWarning("Race with Id: {RaceId} not found", request.Id);
                    return null;
                }

                // Mapování na DTO
                var raceDto = _mapper.Map<RaceDto>(raceEntity);

                _logger.LogInformation(
                    "Race found - Id: {RaceId}, Name: {RaceName}, Category: {RaceCategory}",
                    raceDto.RaceId,
                    raceDto.RaceName,
                    raceDto.RaceCategory);

                return raceDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error querying Race with Id: {RaceId}",
                    request.Id);
                throw; // Re-throw pro vyšší vrstvy (např. API middleware)
            }
        }
    }
}
