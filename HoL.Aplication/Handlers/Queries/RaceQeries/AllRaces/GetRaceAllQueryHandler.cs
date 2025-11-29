using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Queries.GenericQueryes;
using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.Entities;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.AllRaces
{
    /// <summary>
    /// MediatR query handler pro získání všech ras.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Handler dědí z <see cref="GenericAllQueryHandler{TEntity, TDto, TRepository}"/>
    /// a poskytuje konkrétní implementaci pro Race entity.
    /// </para>
    /// <para>
    /// Provádí:
    /// <list type="number">
    /// <item><description>Načtení všech ras z repository</description></item>
    /// <item><description>Mapování na RaceDto</description></item>
    /// <item><description>Logování operací a výsledků</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Použití v API:
    /// <code>
    /// var query = new GetRaceAllQuery();
    /// var results = await _mediator.Send(query);
    /// </code>
    /// </example>
    /// <seealso cref="GetRaceAllQuery"/>
    /// <seealso cref="GenericAllQueryHandler{TEntity, TDto, TRepository}"/>
    public class GetRaceAllQueryHandler :
        GenericAllQueryHandler<Race, RaceDto, IRaceRepository>,
        IRequestHandler<GetRaceAllQuery, IEnumerable<RaceDto>>
    {
        /// <summary>
        /// Inicializuje novou instanci <see cref="GetRaceAllQueryHandler"/>.
        /// </summary>
        /// <param name="repository">Repository pro přístup k Race entitám</param>
        /// <param name="mapper">AutoMapper instance pro mapování entity na DTO</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
        public GetRaceAllQueryHandler(IRaceRepository repository,
                                      IMapper mapper,
                                      ILogger<GetRaceAllQueryHandler> logger)
            : base(repository, mapper, logger, (repo, ct) => repo.ListAsync(ct))
        {
        }

        /// <summary>
        /// Zpracuje query pro získání všech ras.
        /// </summary>
        /// <param name="request">Query request</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>
        /// Kolekce <see cref="RaceDto"/> pro všechny rasy. Prázdná kolekce pokud žádné rasy nebyly nalezeny.
        /// </returns>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k neočekávané chybě</exception>
        public async Task<IEnumerable<RaceDto>> Handle(GetRaceAllQuery request, CancellationToken cancellationToken)
        {
            return await HandleGetAll(cancellationToken, nameof(GetRaceAllQueryHandler));
        }
    }
}
