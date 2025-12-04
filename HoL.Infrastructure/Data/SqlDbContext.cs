using Microsoft.EntityFrameworkCore;
using HoL.Domain;
using HoL.Domain.Entities; // vaše entities

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
            
            // Aplikovat configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlDbContext).Assembly);
        }
    }
}
