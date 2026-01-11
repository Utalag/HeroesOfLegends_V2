using HoL.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HoL.InfrastructureTest.ArrangeData
{
    public class DbInMemoryFactory
    {
        /// <summary>
        /// Vytvoří nový SqlDbContext s In-Memory databází.
        /// Každá databáze je identifikována jedinečným jménem.
        /// </summary>
        /// <param name="dbName">Jedinečné jméno databáze. Pokud je null/empty, použije se GUID.</param>
        /// <returns>Nový SqlDbContext instance s In-Memory databází</returns>
        public static SqlDbContext CreateDbContext(string? dbName = null)
        {
            // Pokud není dbName zadán, použij GUID pro zajištění jedinečnosti
            var databaseName = string.IsNullOrEmpty(dbName)
                ? Guid.NewGuid().ToString()
                : dbName;

            var options = new DbContextOptionsBuilder<SqlDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            return new SqlDbContext(options);
        }

        /// <summary>
        /// Vytvoří nový SqlDbContext s In-Memory databází se stejným jménem.
        /// Užitečné pro připojení k existující In-Memory databázi v testech.
        /// </summary>
        /// <param name="databaseName">Jméno existující In-Memory databáze</param>
        /// <returns>Nový SqlDbContext instance připojený ke stejné databázi</returns>
        public static SqlDbContext CreateDbContextForExistingDatabase(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentException("Database name cannot be empty", nameof(databaseName));

            var options = new DbContextOptionsBuilder<SqlDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            return new SqlDbContext(options);
        }
    }
}
