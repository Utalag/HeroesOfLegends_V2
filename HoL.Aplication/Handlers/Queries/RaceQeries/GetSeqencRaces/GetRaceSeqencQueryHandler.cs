using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.Handlers.Queries.GenericQueryes;
using HoL.Aplication.Interfaces.IRerpositories;
using HoL.Domain.Entities;
using HoL.Domain.Enums;
using MediatR;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetSeqencRaces
{
    /// <summary>
    /// MediatR query handler pro sekvenční (stránkované) získání ras s podporou řazení.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Handler dědí z <see cref="GenericSeqencQueryHandler{TEntity, TDto, TRepository}"/>
    /// a poskytuje konkrétní implementaci pro Race entity.
    /// </para>
    /// <para>
    /// Provádí:
    /// <list type="number">
    /// <item><description>Sekvenční (stránkované) načtení ras z repository</description></item>
    /// <item><description>Podporu řazení podle zadaného pole a směru</description></item>
    /// <item><description>Mapování na RaceDto kolekci</description></item>
    /// <item><description>Logování operací a výsledků</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// <strong>Parametry:</strong>
    /// <list type="bullet">
    /// <item><term>page</term><description>Číslo stránky (1-based)</description></item>
    /// <item><term>size</term><description>Počet ras na stránku</description></item>
    /// <item><term>sortBy</term><description>Pole pro řazení (nullable - pokud null, výchozí řazení)</description></item>
    /// <item><term>sortDir</term><description>Směr řazení (Ascending/Descending)</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Použití v API:
    /// <code>
    /// var query = new GetRaceSeqencQuery(page: 1, size: 10, SortBy: "RaceName", SortDir: SortDirection.Ascending);
    /// var results = await _mediator.Send(query);
    /// </code>
    /// </example>
    /// <seealso cref="GetRaceSeqencQuery"/>
    /// <seealso cref="GenericSeqencQueryHandler{TEntity, TDto, TRepository}"/>
    /// <seealso cref="IRaceRepository"/>
    public class GetRaceSeqencQueryHandler 
        : GenericSeqencQueryHandler<Race, RaceDto, IRaceRepository>,
          IRequestHandler<GetRaceSeqencQuery, IEnumerable<RaceDto>>
    {
        /// <summary>
        /// Inicializuje novou instanci <see cref="GetRaceSeqencQueryHandler"/>.
        /// </summary>
        /// <param name="repository">Repository pro přístup k Race entitám</param>
        /// <param name="mapper">AutoMapper instance pro mapování entity na DTO</param>
        /// <param name="logger">Logger pro zaznamenávání operací</param>
        /// <exception cref="ArgumentNullException">Pokud některý z parametrů je null</exception>
        /// <remarks>
        /// Konstruktor předává base třídě delegát, který volá <see cref="IRaceRepository.GetBySeqencAsync"/>
        /// s parametry pro stránkování a řazení.
        /// </remarks>
        public GetRaceSeqencQueryHandler(IRaceRepository repository,
                                         IMapper mapper,
                                         ILogger<GetRaceSeqencQueryHandler> logger)
            : base(repository, mapper, logger, 
                (repo, page, size, sortBy, sortDir, ct) => repo.GetBySeqencAsync(page, size, sortBy, sortDir, ct))
        {
        }

        /// <summary>
        /// Zpracuje query pro sekvenční získání ras.
        /// </summary>
        /// <param name="request">Query obsahující parametry stránkování a řazení</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>
        /// Kolekce <see cref="RaceDto"/> pro ras na dané stránce. Prázdná kolekce pokud žádné rasy nejsou nalezeny.
        /// </returns>
        /// <remarks>
        /// Metoda deleguje zpracování na base třídu <see cref="GenericSeqencQueryHandler{TEntity, TDto, TRepository}"/>,
        /// která zajišťuje:
        /// <list type="bullet">
        /// <item>Kontrolu cancellation tokenu</item>
        /// <item>Načtení ras z repository</item>
        /// <item>Mapování na DTO</item>
        /// <item>Strukturované logování</item>
        /// <item>Error handling</item>
        /// </list>
        /// </remarks>
        /// <exception cref="OperationCanceledException">Pokud byla operace zrušena</exception>
        /// <exception cref="Exception">Pokud dojde k neočekávané chybě</exception>
        public async Task<IEnumerable<RaceDto>> Handle(GetRaceSeqencQuery request, CancellationToken cancellationToken)
        {
            return await HandleSeqencData(
                request.page,
                request.size,
                request.SortBy,
                request.SortDir,
                cancellationToken,
                nameof(GetRaceSeqencQueryHandler));
        }
    }
}
