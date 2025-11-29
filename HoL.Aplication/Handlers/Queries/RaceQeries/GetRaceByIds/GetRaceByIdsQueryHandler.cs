using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Queries.GenericQueryes;
using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.Entities;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetRaceByIds
{
    /// <summary>
    /// MediatR handler pro získání ras podle kolekce ID.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Handler dědí z <see cref="GenericIdsQueryHandler{TEntity, TDto, TRepository}"/>
    /// a poskytuje konkrétní implementaci pro Race entity.
    /// </para>
    /// <para>
    /// Provádí:
    /// <list type="number">
    /// <item><description>Validaci a normalizaci vstupních ID</description></item>
    /// <item><description>Načtení ras z repository</description></item>
    /// <item><description>Mapování na RaceDto</description></item>
    /// <item><description>Logování operací a výsledků</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Použití v API:
    /// <code>
    /// var query = new GetRacesByIdsQuery { Ids = new[] { 1, 2, 3 } };
    /// var results = await _mediator.Send(query);
    /// </code>
    /// </example>
    /// <seealso cref="GetRacesByIdsQuery"/>
    /// <seealso cref="IRaceRepository"/>
    public class GetRaceByIdsQueryHandler
        : GenericIdsQueryHandler<Race, RaceDto, IRaceRepository>,
          IRequestHandler<GetRacesByIdsQuery, IEnumerable<RaceDto>>
    {
        /// <summary>
        /// Inicializuje novou instanci <see cref="GetRaceByIdsQueryHandler"/>.
        /// </summary>
        /// <param name="raceRepository">Repository pro přístup k Race entitám</param>
        /// <param name="mapper">AutoMapper instance pro mapování entity na DTO</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
        public GetRaceByIdsQueryHandler(IRaceRepository raceRepository,
                                        IMapper mapper,
                                        ILogger<GetRaceByIdsQueryHandler> logger)
            : base(raceRepository, mapper, logger, (repo, id, ct) => repo.GetByIdAsync(id, ct))
        {
        }

        /// <summary>
        /// Zpracuje query pro získání ras podle kolekce ID.
        /// </summary>
        /// <param name="request">Query obsahující kolekci ID k načtení</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>
        /// Kolekce <see cref="RaceDto"/> pro nalezené rasy. Prázdná kolekce pokud žádné rasy nebyly nalezeny.
        /// </returns>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k neočekávané chybě</exception>
        public async Task<IEnumerable<RaceDto>> Handle(GetRacesByIdsQuery request, CancellationToken cancellationToken)
        {
            return await HandleGetByIds(request.Ids, cancellationToken,nameof(GetRaceByIdsQueryHandler));
        }
    }
}
