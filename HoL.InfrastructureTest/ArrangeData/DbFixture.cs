using Moq;
using HoL.Domain.Entities.CurencyEntities;
using HoL.Infrastructure.Data;
using HoL.Infrastructure.Data.Models;
using HoL.Infrastructure.Repositories;
using AutoMapper;
using HoL.Infrastructure.Data.MapModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HoL.InfrastructureTest.ArrangeData
{
    public class DbFixture
    {
        public SqlDbContext Context { get; }

        public DbFixture()
        {
            var options = new DbContextOptionsBuilder<SqlDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb_For_SingleCurrencyReadMethod")
                .Options;
            Context = new SqlDbContext(options);
            Seed(Context);
        }

        private void Seed(SqlDbContext context)
        {
            // Přidáme pouze CurrencyGroups - SingleCurrencies jsou součástí CurrencyGroup
            context.CurrencyGroups.Add(ArrangeClass.CurrencyGroupDbModel_Arrange());
            context.SaveChanges();

            // Poté přidáme Races (které obsahují Treasure s referencemi na CurrencyGroup)
            context.Races.AddRange(ArrangeClass.RaceDbModel_Arrange());
            context.SaveChanges();
        }

    }
}
