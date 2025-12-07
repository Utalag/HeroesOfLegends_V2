using HoL.Aplication.DTOs;
using HoL.Domain.Entities;
using HoL.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace HoL.Aplication.Interfaces.IRerpositories
{
    public interface IRaceRepository
    {
        /// <summary>
        /// Přidá novou rasu. Implementace by měla nastavit Race.RaceId po persistenci.
        /// </summary>
        Task AddAsync(Race race, CancellationToken cancellationToken = default);

        /// <summary>
        /// Aktualizuje existující rasu.
        /// </summary>
        Task UpdateAsync(Race race, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Smaže rasu podle id.
        /// </summary>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        



        /// <summary>
        /// Rychlá kontrola existence záznamu.
        /// </summary>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);





        /// <summary>
        /// Vrátí rasu podle id nebo null pokud neexistuje.
        /// </summary>
        Task<Race?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Race?>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Vrátí všechny rasy (pro jednoduchost bez stránkování).
        /// </summary>
        Task<IEnumerable<Race>> ListAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// Volitelně: najde rasu podle názvu (užitečné pro validaci duplicit před vytvořením).
        /// </summary>
        Task<Race?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<IEnumerable<Race>> GetBySeqencAsync(
            int Page=1,
            int Size=5,
            string ?SortBy = default,
            SortDirection SortDir = SortDirection.Original, 
            CancellationToken cancellationToken = default);
    }
}