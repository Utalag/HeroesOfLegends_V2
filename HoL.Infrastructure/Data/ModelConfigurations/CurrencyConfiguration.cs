using HoL.Domain.Entities.CurencyEntities;
using HoL.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HoL.Infrastructure.Data.ModelConfigurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<CurrencyGroupDbModel>
    {
        public void Configure(EntityTypeBuilder<CurrencyGroupDbModel> builder)
        {
            builder.ToTable("CurrencyGroups");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            
            builder.Property(c => c.GroupName)
                .IsRequired()
                .HasMaxLength(100);

            // Konfigurace Many-to-Many vztahu
            // Jedna skupina může obsahovat více měn A jedna měna může být ve více skupinách
            builder.HasMany(c => c.Currencies)
                .WithMany() // Many-to-Many bez navigační vlastnosti na SingleCurrency straně
                
                .UsingEntity<Dictionary<string, object>>("CurrencyGroupSingleCurrency", // Název spojovací tabulky
                    j => j.HasOne<SingleCurrencyDbModel>().WithMany().HasForeignKey("SingleCurrencyId"),
                    j => j.HasOne<CurrencyGroupDbModel>().WithMany().HasForeignKey("CurrencyGroupId"),
                    j =>
                    {
                        // Konfigurace spojovací tabulky
                        j.HasKey("CurrencyGroupId", "SingleCurrencyId"); // Složený primární klíč
                        j.ToTable("CurrencyGroupSingleCurrency");
                    });

            // Index pro rychlejší vyhledávání podle názvu skupiny
            builder.HasIndex(c => c.GroupName).IsUnique();
        }
    }
}
