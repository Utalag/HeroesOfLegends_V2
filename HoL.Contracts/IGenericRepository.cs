namespace HoL.Contracts
{
    /// <summary>
    /// Generické rozhraní pro repository operace na entitách.
    /// Poskytuje základní CRUD operace a dotazy pro přístup k datům.
    /// </summary>
    /// <typeparam name="TEntity">Typ entity (musí být referenční typ)</typeparam>
    /// <typeparam name="TKey">Typ primárního klíče entity</typeparam>
    public interface IGenericRepository<TEntity, TKey> 
        where TEntity : class
    {
        /// <summary>
        /// Přidá novou entitu do databáze.
        /// </summary>
        /// <param name="entity">Entita k přidání (nesmí být null)</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>Přidaná entita s nastaveným primárním klíčem</returns>
        Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Aktualizuje existující entitu v databázi.
        /// </summary>
        /// <param name="entity">Entita s aktualizovanými hodnotami (nesmí být null)</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>Aktualizovaná entita</returns>
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Smaže entitu z databáze podle primárního klíče.
        /// </summary>
        /// <param name="id">Primární klíč entity k smazání</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>Task reprezentující asynchronní operaci</returns>
        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Kontroluje zda entita s daným primárním klíčem existuje.
        /// </summary>
        /// <param name="id">Primární klíč entity k ověření</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>True pokud entita existuje, jinak false</returns>
        Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Získá entitu podle primárního klíče.
        /// </summary>
        /// <param name="id">Primární klíč entity k načtení</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>Entita pokud existuje, jinak null</returns>
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Načte všechny entity z databáze.
        /// </summary>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>Kolekce všech entit, prázdná kolekce pokud žádné neexistují</returns>
        Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Hledá entitu podle jména.
        /// </summary>
        /// <param name="name">Jméno entity k vyhledání</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>Entita pokud existuje, jinak null</returns>
        Task<TEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Načte stránku entit z databáze s podporou stránkování.
        /// </summary>
        /// <param name="Page">Číslo stránky (počítáno od 1), výchozí hodnota 1</param>
        /// <param name="Size">Počet entit na stránku, výchozí hodnota 5</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>Kolekce entit na požadované stránce, prázdná kolekce pokud stránka je mimo rozsah</returns>
        Task<IEnumerable<TEntity>> GetPageAsync(
            int Page = 1,
            int Size = 5,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Načte entity podle seznamu primárních klíčů.
        /// </summary>
        /// <param name="ids">Seznam primárních klíčů</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>Kolekce entit které odpovídají daným ID, prázdná kolekce pokud žádné neexistují</returns>
        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Načte entit s podporou stránkování a řazení.
        /// </summary>
        /// <param name="page">Číslo stránky (počítáno od 1)</param>
        /// <param name="size">Počet entit na stránku</param>
        /// <param name="sortBy">Jméno vlastnosti pro řazení</param>
        /// <param name="sortDir">Směr řazení (asc/desc)</param>
        /// <param name="cancellationToken">Token pro zrušení operace</param>
        /// <returns>Kolekce entit na požadované stránce seřazené podle kritérií</returns>
        Task<IEnumerable<TEntity>> GetBySeqencAsync(
            int page,
            int size,
            string? sortBy = null,
            string? sortDir = null,
            CancellationToken cancellationToken = default);
    }
}