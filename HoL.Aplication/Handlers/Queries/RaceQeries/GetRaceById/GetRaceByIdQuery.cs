using HoL.Aplication.DTOs.EntitiDtos;
using MediatR;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetRaceById
{
    /// <summary>
    /// MediatR query pro získání rasy podle ID.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Read-only query pro načtení jedné rasy z databáze.
    /// Vrací <see cref="RaceDto"/> nebo <c>null</c> pokud rasa neexistuje.
    /// </para>
    /// <para>
    /// Query pattern - nemění stav systému, pouze čte data.
    /// </para>
    /// </remarks>
    /// <example>
    /// Použití v API controlleru:
    /// <code>
    /// [HttpGet("{id}")]
    /// public async Task&lt;IActionResult&gt; GetRace(int id)
    /// {
    ///     var query = new GetRaceByIdQuery(id);
    ///     var result = await _mediator.Send(query);
    ///     return result != null ? Ok(result) : NotFound();
    /// }
    /// </code>
    /// </example>
    /// <param name="Id">ID rasy k načtení (musí být > 0)</param>
    /// <seealso cref="GetRaceByIdQueryHandler"/>
    /// <seealso cref="GetRaceByIdQueryValidator"/>
    public sealed record GetRaceByIdQuery(int Id) : IRequest<RaceDto?>;
}
