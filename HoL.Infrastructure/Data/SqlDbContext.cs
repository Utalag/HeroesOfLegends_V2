using HoL.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HoL.Infrastructure.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {
        }

        // Hlavní entity
        public DbSet<RaceDbModel> Races { get; set; }
        public DbSet<CurrencyGroupDbModel> CurrencyGroups { get; set; }
        public DbSet<SingleCurrencyDbModel> SingleCurrencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplikovat configurations z assembly - konfigurace entit v samostatných třídách
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlDbContext).Assembly);
        }
    }
}
