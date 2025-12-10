using HoL.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HoL.Infrastructure.Data
{
    public class SqlDbContext : DbContext
    {


        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {
        }

        public DbSet<Race> Races { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplikovat configurations z assembly - pokud máte IEntityTypeConfiguration soubory
            // konfigurace Entit v samostatných třídách
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlDbContext).Assembly);

        }


    }
}
